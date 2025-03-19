using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Model
{
    [JsonProperty("count_operation")]
    public int CountOperation { get; set; }

    [JsonProperty("count_types_of_operations")]
    public int CountTypesOfOperations { get; set; }

    [JsonProperty("types")]
    public Dictionary<string, List<int>> Types { get; set; }

    [JsonProperty("count_processors")]
    public Dictionary<string, int> CountProcessors { get; set; }

    [JsonProperty("time_of_calculation")]
    public Dictionary<string, int> TimeOfCalculation { get; set; }

    [JsonProperty("table_H")]
    public List<List<int>> TableH { get; set; }
}
