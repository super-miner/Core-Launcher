using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace CoreLauncher.Scripts.ModIO.JsonStructures; 

public class DependencyListInfo {
	[JsonInclude] [JsonPropertyName("data")] public List<DependencyInfo> Dependencies = null;

	public List<int> GetDependencies() {
		List<int> output = new List<int>();

		foreach (DependencyInfo dependency in Dependencies) {
			output.Add(dependency.Id);
		}

		return output;
	}
}
