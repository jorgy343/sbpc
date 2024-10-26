using Sbpc.Serialization;

Blueprint blueprint = Serialization.ReadBlueprintFile("../../../../../samples/OneBlock.sbp");

Serialization.WriteBlueprintFile("../../../../../samples/TEST.sbp", blueprint);