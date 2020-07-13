<Query Kind="Statements">
  <Output>DataGrids</Output>
</Query>

var parammeters = new { a = 1, b =2};


foreach(PropertyInfo parameter in parammeters.GetType().GetProperties())
{
	parameter.Name.Dump();
	parameter.GetValue(parammeters).Dump();
	var x = parameter.GetValue(parammeters).Dump();
	Type t = x.GetType();
	if(t.Equals(typeof(int)))
	{
		"True".Dump();
	}
}