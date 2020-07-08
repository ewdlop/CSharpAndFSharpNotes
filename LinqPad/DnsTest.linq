<Query Kind="Statements" />

var domainEntry = System.Net.Dns.GetHostEntry("google.com");
domainEntry.Dump();
foreach(var ip in domainEntry.AddressList)
{
	ip.Dump();
}