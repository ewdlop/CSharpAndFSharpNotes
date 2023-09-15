
using HL7.Dotnetcore;
///https://docs.fire.ly/projects/Firely-NET-SDK/start.html

string hl7Message = """
MSH|^~\&|ADT1|MCM|LABADT|MCM|198808181126|SECURITY|ADT^A01|MSG00001|P|2.6
EVN|A01|198808181123||
PID|1|461 19 1953||DOE^JOHN^^^^|DOE^JOHN^^^^|19480203|M|||123 MAIN STREET^^GREENSBORO^NC^^27401-4901^^M^^||(919)379-1212|(919)271-3434||S||PATID12345001^2^M10^ADT1^MR^VISIT 1^20070822|123456789|9-87654^NC
NK1|1|DOE^JANE|SPO||(919)555-5555||NK^NEXT OF KIN
PV1|1|I|2000^2012^01||||004777^FISHER^BEN^J.|||||||||||V100^I9|||||||||||||||||||||||||||20070822
""";

Message message = new Message(hl7Message);
message.ParseMessage();
List<Segment> segList = message.Segments();
foreach(Segment segment in segList)
{
    Console.WriteLine(segment.Value);
}