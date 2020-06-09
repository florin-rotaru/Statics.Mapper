using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Xunit;

namespace Builder
{
    public class FromTo_Builder
    {
        #region internal
        class TNode
        {
            public string Group { get; set; }
            public bool IsValueType { get; set; }
            public bool IsNullable { get; set; }
            public bool IsStatic { get; set; }
            public string Name { get; set; }

            public TNode() { }

            public TNode(string group, bool isValueType, bool isNullable, bool isStatic, string name)
            {
                Group = group;
                IsValueType = isValueType;
                IsNullable = isNullable;
                IsStatic = isStatic;
                Name = name;
            }

            public TNode(string group, bool isValueType, bool isNullable, bool isStatic) :
                this(group, isValueType, isNullable, isStatic, null)
            { }
        }

        class TModel
        {
            public string Prefix { get; set; }
            public bool IsValueType { get; set; }
            public TNode[] TNodes { get; set; }

            public TModel(string prefix, bool isValueType, TNode[] nodes)
            {
                Prefix = prefix;
                IsValueType = isValueType;
                TNodes = nodes;
            }
        }

        static readonly TModel[] TModels = new TModel[]
        {
          new TModel("C", false, new TNode[]
          {
              new TNode("C", false, false, false),
              new TNode("S", true, false, false),
              new TNode("NS", true, true, false)
          }),
          new TModel("S", true, new TNode[]
          {
              new TNode("C", false, false, false),
              new TNode("S", true, false, false),
              new TNode("NS", true, true, false)
          }),
          new TModel("C", false, new TNode[]
          {
              new TNode("SC", false, false, true),
              new TNode("SS", true, false, true),
              new TNode("SNS", true, true, true)
          }),
          new TModel("S", true, new TNode[]
          {
              new TNode("SC", false, false, true),
              new TNode("SS", true, false, true),
              new TNode("SNS", true, true, true)
          })
        };

        string IsNullable(bool nullable, string result) =>
            nullable ? result : string.Empty;

        string Lower(object text) =>
            text.ToString().ToLower();

        string Capitalize(string text) =>
            char.ToUpper(text[0]) + text.Substring(1);

        string Tabs(int n) =>
            new string('\t', n);

        void InitDirectory(string directory)
        {
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
        }

        void WriteToFile(StringBuilder builder, string file) =>
             File.WriteAllText(file, builder.ToString());

        string BuilderFile(string fileName) =>
            Path.Combine(BuilderPath(), fileName);

        string ModelsFile(string fileName) =>
            Path.Combine(ModelsPath(), fileName);

        string TestsPath() =>
            Directory.GetCurrentDirectory().Split(nameof(Builder))[0];

        string BuilderPath() =>
            Path.Combine(Directory.GetCurrentDirectory().Split(nameof(Builder))[0], nameof(Builder));

        string ModelsPath() =>
            Path.Combine(Directory.GetCurrentDirectory().Split(nameof(Builder))[0], nameof(Models));

        bool ContainsReadOnlyMembers(TNode node) =>
            node.Name.Split(new string[] { "Readonly", "Literal" }, StringSplitOptions.RemoveEmptyEntries).Length > 1;

        bool ContainsStaticMembers(TNode node) =>
            node.Name.Contains("Static");

        List<TNode> BuildModels()
        {
            File.WriteAllText(ModelsFile("Models.N0.cs"), File.ReadAllText(BuilderFile("Models.N0.cs")));

            var nodeTypes = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t =>
                    t.Namespace == nameof(Models) &&
                    new string[] { "TC0", "TS0" }.Contains(t.Name.Split('_')[0]))
                .Select(t =>
                    new TNode(
                        t.Name.Split('_')[0].Substring(1),
                        t.IsValueType,
                        false,
                        false,
                        t.Name))
                .ToList();

            for (int depth = 1; depth < 3; depth++)
            {
                var builder = new StringBuilder();

                builder
                    .AppendLine()
                    .AppendLine($"namespace {nameof(Models)}")
                    .AppendLine("{");

                foreach (var modelTemplate in TModels)
                {
                    foreach (var nodeTemplate in modelTemplate.TNodes)
                    {
                        var nodeTypesByTemplateGroup = nodeTypes
                            .Where(m => m.Group.StartsWith($"{nodeTemplate.Group}{depth - 1}"))
                            .ToList();

                        if (nodeTypesByTemplateGroup.Count == 0)
                        {
                            nodeTypesByTemplateGroup = nodeTemplate.Group switch
                            {
                                "NS" => nodeTypes
                                    .Where(m => m.Group.StartsWith($"S{depth - 1}"))
                                    .Select(s => new TNode($"N{s.Group}", s.IsValueType, true, s.IsStatic, s.Name))
                                    .ToList(),
                                "SC" => nodeTypes
                                    .Where(m => m.Group.StartsWith($"C{depth - 1}"))
                                    .Select(s => new TNode($"S{s.Group}", s.IsValueType, s.IsNullable, true, s.Name))
                                    .ToList(),
                                "SS" => nodeTypes
                                    .Where(m => m.Group.StartsWith($"S{depth - 1}"))
                                    .Select(s => new TNode($"S{s.Group}", s.IsValueType, s.IsNullable, true, s.Name))
                                    .ToList(),
                                "SNS" => nodeTypes
                                    .Where(m => m.Group.StartsWith($"S{depth - 1}"))
                                    .Select(s => new TNode($"SN{s.Group}", s.IsValueType, true, true, s.Name))
                                    .ToList(),
                                _ => throw new Exception(),
                            };
                        }

                        var tModelType = modelTemplate.IsValueType ? "struct" : "class";
                        var tModelTypeDescription = Capitalize(tModelType);
                        var tNodeTypeDescription =
                            (nodeTemplate.IsStatic ? " Static" : string.Empty) +
                            (nodeTemplate.IsNullable ? " Nullable" : string.Empty) +
                            (nodeTemplate.IsValueType ? " Struct" : " Class");

                        var nodeTypesGroups = nodeTypesByTemplateGroup.GroupBy(k => k.Group);

                        foreach (var group in nodeTypesGroups)
                        {
                            string groupName = $"{modelTemplate.Prefix}{depth}{group.Key}";

                            builder.AppendLine($"{Tabs(1)}#region {groupName} " +
                                $"{Capitalize(tModelTypeDescription)} with" +
                                $"{Capitalize(tNodeTypeDescription)} Node");

                            var newNodeTypes = new List<TNode>();

                            foreach (var nodeType in group)
                            {
                                newNodeTypes.Add(
                                    new TNode(
                                        groupName,
                                        modelTemplate.IsValueType,
                                        false,
                                        false,
                                        $"T{groupName}_{nodeType.Name.Split('_', 2)[1]}"));

                                builder
                                    .AppendLine($"{Tabs(1)}public {tModelType} " +
                                    $"T{groupName}_{nodeType.Name.Split('_', 2)[1]} " +
                                    $"{{ public " +
                                    $"{(nodeTemplate.IsStatic ? "static " : string.Empty)}" +
                                    $"{nodeType.Name}" +
                                    $"{IsNullable(nodeTemplate.IsNullable, "?")} " +
                                    $"N{depth - 1} {{ get; set; }} }}");
                            }

                            nodeTypes.AddRange(newNodeTypes);

                            builder.AppendLine($"{Tabs(1)}#endregion");
                        }
                    }
                }

                builder.AppendLine("}");
                WriteToFile(builder, ModelsFile($"Models.N{depth}.cs"));
            }

            return nodeTypes;
        }

        void BuildToNodesGroup(
            ref StringBuilder builder,
            ref StringBuilder nonStaticBuilder,
            ref StringBuilder staticBuilder,
            int depth,
            string group,
            bool isNullable,
            List<TNode> nodes)
        {
            var groupName = $"{IsNullable(isNullable, "N")}{group}";

            builder.AppendLine($"{Tabs(2)}#region To {groupName}");
            nonStaticBuilder.AppendLine($"{Tabs(2)}#region To {groupName}");
            staticBuilder.AppendLine($"{Tabs(2)}#region To {groupName}");

            foreach (var node in nodes)
            {
                var containsStaticMembers = ContainsStaticMembers(node);

                var intermediate = new StringBuilder();

                intermediate
                    .AppendLine($"{Tabs(2)}[Fact]")
                    .AppendLine($"{Tabs(2)}public void To_{IsNullable(isNullable, "N")}{node.Name.Substring(1)}() => To" +
                    $"{(isNullable ? "NullableStruct" : node.IsValueType ? "Struct" : "Class")}<{node.Name}>" +
                    $"({Lower(ContainsReadOnlyMembers(node))}{(depth == 0 ? $", {Lower(containsStaticMembers)}" : string.Empty)});");

                builder.Append($"{intermediate}");

                if (ContainsStaticMembers(node) || ContainsStaticNodes(node))
                    staticBuilder.Append($"{intermediate}");
                else
                    nonStaticBuilder.Append($"{intermediate}");
            }

            builder.AppendLine($"{Tabs(2)}#endregion");
            nonStaticBuilder.AppendLine($"{Tabs(2)}#endregion");
            staticBuilder.AppendLine($"{Tabs(2)}#endregion");
        }

        List<TNode> BuildFromToN()
        {
            var nodeTypes = BuildModels();

            File.WriteAllText(ModelsFile("FromTo_N0.cs"), File.ReadAllText(BuilderFile("FromTo_N0.cs")));
            File.WriteAllText(ModelsFile("FromTo_N1.cs"), File.ReadAllText(BuilderFile("FromTo_N1.cs")));
            File.WriteAllText(ModelsFile("FromTo_N2.cs"), File.ReadAllText(BuilderFile("FromTo_N2.cs")));

            for (int depth = 0; depth < nodeTypes.GroupBy(k => k.Group[1]).Count(); depth++)
            {
                var builder = new StringBuilder()
                    .AppendLine("using Models;")
                    .AppendLine("using Xunit;")
                    .AppendLine("using Xunit.Abstractions;")
                    .AppendLine()
                    .AppendLine("namespace Internal")
                    .AppendLine("{");
                var nonStaticBuilder = new StringBuilder(builder.ToString());
                var staticBuilder = new StringBuilder(builder.ToString());

                builder.AppendLine($"{Tabs(1)}public class FromTo_N{depth}_Members<S> : FromTo_N{depth}<S> where S : new()")
                   .AppendLine($"{Tabs(1)}{{")
                   .AppendLine($"{Tabs(2)}public FromTo_N{depth}_Members(ITestOutputHelper console) : base(console) {{ }}")
                   .AppendLine();
                nonStaticBuilder.AppendLine($"{Tabs(1)}public class FromTo_N{depth}_NonStatic_Members<S> : FromTo_N{depth}<S> where S : new()")
                    .AppendLine($"{Tabs(1)}{{")
                    .AppendLine($"{Tabs(2)}public FromTo_N{depth}_NonStatic_Members(ITestOutputHelper console) : base(console) {{ }}")
                    .AppendLine();
                staticBuilder.AppendLine($"{Tabs(1)}public class FromTo_N{depth}_Static_Members<S> : FromTo_N{depth}<S> where S : new()")
                    .AppendLine($"{Tabs(1)}{{")
                    .AppendLine($"{Tabs(2)}public FromTo_N{depth}_Static_Members(ITestOutputHelper console) : base(console) {{ }}")
                    .AppendLine();

                foreach (var group in nodeTypes.Where(n => $"{n.Group[1]}" == $"{depth}").GroupBy(n => n.Group))
                {
                    BuildToNodesGroup(ref builder, ref nonStaticBuilder, ref staticBuilder, depth, group.Key, false, group.ToList());

                    if (group.Key[0] == 'S')
                        BuildToNodesGroup(ref builder, ref nonStaticBuilder, ref staticBuilder, depth, group.Key, true, group.ToList());
                }

                builder.AppendLine($"{Tabs(1)}}}");
                nonStaticBuilder.AppendLine($"{Tabs(1)}}}");
                staticBuilder.AppendLine($"{Tabs(1)}}}");

                builder.AppendLine("}");
                nonStaticBuilder.AppendLine("}");
                staticBuilder.AppendLine("}");

                WriteToFile(builder, ModelsFile($"FromTo_N{depth}_Members.cs"));
                WriteToFile(nonStaticBuilder, ModelsFile($"FromTo_N{depth}_NonStatic_Members.cs"));
                WriteToFile(staticBuilder, ModelsFile($"FromTo_N{depth}_Static_Members.cs"));
            }

            return nodeTypes;
        }

        void BuildXunitRunnerJson(string directory)
        {
            if (File.Exists(Path.Combine(directory, "xunit.runner.json")))
                return;

            File.WriteAllText(
                Path.Combine(directory, "xunit.runner.json"),
                File.ReadAllText(BuilderFile("xunit.runner.json.file"))
            );
        }

        void Buildcsproj(string directory, string group)
        {
            string csproj = Path.Combine(directory, $"{group}.csproj");
            if (File.Exists(csproj))
                return;

            File.WriteAllText(
                csproj,
                File.ReadAllText(BuilderFile("group.csproj.file"))
            );
        }

        bool ContainsStaticNodes(TNode node)
        {
            List<string> nodes = new List<string>();
            string block = string.Empty;

            foreach (char c in node.Group)
            {
                block += c;
                if (char.IsDigit(c))
                {
                    nodes.Add(block);
                    block = string.Empty;
                }
            }

            return nodes.Exists(n => n.StartsWith('S') && char.IsLetter(n[1]));
        }

        void BuildFromNonStaticModelMembersTest(ref StringBuilder builder, TNode model, bool nullable)
        {
            if (ContainsStaticNodes(model))
                builder.AppendLine($"{Tabs(1)}[Collection(\"S_{model.Group}\")]");

            builder
                .AppendLine($"{Tabs(1)}public class From_NS_{IsNullable(nullable, "N")}{model.Name.Substring(1)} : FromTo_N{model.Name[2]}_NonStatic_Members<{model.Name}{IsNullable(nullable, "?")}> " +
                $"{{ public From_NS_{IsNullable(nullable, "N")}{model.Name.Substring(1)}(ITestOutputHelper console) : base(console) {{}} }}")
                .AppendLine()
                .AppendLine($"{Tabs(1)}[Collection(\"S_{model.Group}\")]")
                .AppendLine($"{Tabs(1)}public class From_S_{IsNullable(nullable, "N")}{model.Name.Substring(1)} : FromTo_N{model.Name[2]}_Static_Members<{model.Name}{IsNullable(nullable, "?")}>" +
                $"{{ public From_S_{IsNullable(nullable, "N")}{model.Name.Substring(1)}(ITestOutputHelper console) : base(console) {{}} }}")
                .AppendLine();
        }

        void BuildFromStaticModelMembersTest(ref StringBuilder builder, TNode model, bool nullable)
        {
            builder
                .AppendLine($"{Tabs(1)}[Collection(\"S_{model.Group}\")]")
                .AppendLine($"{Tabs(1)}public class From_{IsNullable(nullable, "N")}{model.Name.Substring(1)} : FromTo_N{model.Name[2]}_Members<{model.Name}{IsNullable(nullable, "?")}> " +
                $"{{ public From_{IsNullable(nullable, "N")}{model.Name.Substring(1)}(ITestOutputHelper console) : base(console) {{}} }}")
                .AppendLine();
        }

        void BuildModelTest(ref StringBuilder builder, TNode model, bool nullable)
        {
            if (ContainsStaticMembers(model))
                BuildFromStaticModelMembersTest(ref builder, model, nullable);
            else
                BuildFromNonStaticModelMembersTest(ref builder, model, nullable);
        }

        void BuildGroupTests(IGrouping<string, TNode> group, string path, bool nullable)
        {
            var groupDirectory = Path.Combine(path, $"{IsNullable(nullable, "N")}{group.Key}");

            InitDirectory(groupDirectory);
            BuildXunitRunnerJson(groupDirectory);
            Buildcsproj(groupDirectory, $"{IsNullable(nullable, "N")}{group.Key}");

            string[] libs = new string[]
            {
                "Internal",
                "Models",
                "Xunit",
                "Xunit.Abstractions"
            };

            var builder = new StringBuilder();

            for (int i = 0; i < libs.Length; i++)
                builder.AppendLine($"using {libs[i]};");

            builder.AppendLine();

            builder
                .AppendLine($"namespace {IsNullable(nullable, "N")}{group.Key}")
                .AppendLine("{");

            foreach (TNode model in group)
                BuildModelTest(ref builder, model, nullable);

            builder
                .AppendLine("}");

            File.WriteAllText(
                Path.Combine(groupDirectory, $"From_{IsNullable(nullable, "N")}{group.Key}.cs"),
                builder.ToString());
        }

        string CsprosSlnEntry(string path, string group, bool nullable) =>
            $"Project(\"{{9A19103F-16F7-4668-BE54-9A1E7A4F7556}}\") = \"{IsNullable(nullable, "N")}{group}\", " +
            $"\"Tests{path.Split("Tests")[1]}\\{IsNullable(nullable, "N")}{group}\\{IsNullable(nullable, "N")}{group}.csproj\", " +
            $"\"{{{Guid.NewGuid().ToString().ToUpper()}}}\"" +
            "\r\n" +
            "EndProject";

        #endregion

        // [Fact]
        public void Build()
        {
            var nodeTypes = BuildFromToN();

            for (int depth = 0; depth < nodeTypes.GroupBy(k => k.Group[1]).Count(); depth++)
            {
                var slnBuilder = new StringBuilder();
                var path = Path.Combine(TestsPath(), $"N{depth}");
                InitDirectory(path);

                var groups = nodeTypes
                    .Where(n => $"{n.Group[1]}" == $"{depth}")
                    .GroupBy(k => k.Group);

                foreach (var group in groups)
                {
                    BuildGroupTests(group, path, false);
                    slnBuilder.AppendLine(CsprosSlnEntry(path, group.Key, false));

                    if (group.Key.StartsWith('S'))
                    {
                        BuildGroupTests(group, path, true);
                        slnBuilder.AppendLine(CsprosSlnEntry(path, group.Key, true));
                    }
                }

                File.WriteAllText(
                    Path.Combine(path, $"sln.file"),
                    slnBuilder.ToString());
            }
        }
    }
}
