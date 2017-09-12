using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.CommonSrc.Configuration
{
    public class Regions
    {
        //private Dictionary<int, string> _regions = new Dictionary<int, string>();
        private List<Region> _regions = new List<Region>();

		public Regions()
		{
            _regions.Add(new Region(168, "Aberdeen"));
            _regions.Add(new Region(169, "Aberdeenshire"));
            _regions.Add(new Region(2, "Anglesey"));
            _regions.Add(new Region(170, "Angus"));
            _regions.Add(new Region(171, "Argyll"));
            _regions.Add(new Region(101, "Avon"));
            _regions.Add(new Region(5, "Bedfordshire"));
            _regions.Add(new Region(6, "Berkshire"));
            _regions.Add(new Region(172, "Blaenau"));
            _regions.Add(new Region(102, "Borders"));
            _regions.Add(new Region(10, "Buckinghamshire"));
            _regions.Add(new Region(173, "Caerphilly"));
            _regions.Add(new Region(12, "Cambridgeshire"));
            _regions.Add(new Region(174, "Cardiff"));
            _regions.Add(new Region(175, "Carmarthenshire"));
            _regions.Add(new Region(103, "Central(Scotland)"));
            _regions.Add(new Region(176, "Ceredigion"));
            _regions.Add(new Region(104, "Channel Islands"));
            _regions.Add(new Region(16, "Cheshire"));
            _regions.Add(new Region(177, "Clackmannanshire"));
            _regions.Add(new Region(17, "Cleveland"));
            _regions.Add(new Region(18, "Clwyd"));
            _regions.Add(new Region(211, "Connaught"));
            _regions.Add(new Region(178, "Conwy"));
            _regions.Add(new Region(19, "Cornwall"));
            _regions.Add(new Region(20, "Cumbria"));
            _regions.Add(new Region(210, "Denbighshire"));
            _regions.Add(new Region(22, "Derbyshire"));
            _regions.Add(new Region(23, "Devon"));
            _regions.Add(new Region(24, "Dorset"));
            _regions.Add(new Region(25, "Dumfries & Galloway"));
            _regions.Add(new Region(179, "Dundee"));
            _regions.Add(new Region(27, "Durham"));
            _regions.Add(new Region(29, "Dyfed"));
            _regions.Add(new Region(180, "East Ayrshire"));
            _regions.Add(new Region(181, "East Dumbartonshire"));
            _regions.Add(new Region(182, "East Lothian"));
            _regions.Add(new Region(183, "East Renfrewshire"));
            _regions.Add(new Region(33, "East Sussex"));
            _regions.Add(new Region(184, "East Yorkshire"));
            _regions.Add(new Region(185, "Edinburgh"));
            _regions.Add(new Region(35, "Essex"));
            _regions.Add(new Region(209, "Falkirk"));
            _regions.Add(new Region(37, "Fife"));
            _regions.Add(new Region(187, "Flintshire"));
            _regions.Add(new Region(188, "Glasgow"));
            _regions.Add(new Region(40, "Gloucestershire"));
            _regions.Add(new Region(111, "Grampian"));
            _regions.Add(new Region(112, "Gwent"));
            _regions.Add(new Region(43, "Gwynedd"));
            _regions.Add(new Region(44, "Hampshire"));
            _regions.Add(new Region(45, "Herefordshire"));
            _regions.Add(new Region(46, "Hertfordshire"));
            _regions.Add(new Region(47, "Highlands"));
            _regions.Add(new Region(48, "Humberside"));
            _regions.Add(new Region(189, "Inverclyde"));
            _regions.Add(new Region(113, "Isle Of Man"));
            _regions.Add(new Region(49, "Isle of Wight"));
            _regions.Add(new Region(50, "Kent"));
            _regions.Add(new Region(51, "Lancashire"));
            _regions.Add(new Region(52, "Leicestershire"));
            _regions.Add(new Region(212, "Leinster"));
            _regions.Add(new Region(53, "Lincolnshire"));
            _regions.Add(new Region(41, "London"));
            _regions.Add(new Region(114, "Lothian"));
            _regions.Add(new Region(42, "Manchester"));
            _regions.Add(new Region(54, "Merseyside"));
            _regions.Add(new Region(190, "Merthyr"));
            _regions.Add(new Region(55, "Mid Glamorgan"));
            _regions.Add(new Region(191, "Monmouthshire"));
            _regions.Add(new Region(192, "Moray"));
            _regions.Add(new Region(213, "Munster"));
            _regions.Add(new Region(193, "Neath"));
            _regions.Add(new Region(194, "Newport"));
            _regions.Add(new Region(115, "Norfolk"));
            _regions.Add(new Region(195, "North Ayrshire"));
            _regions.Add(new Region(196, "North Lanarkshire"));
            _regions.Add(new Region(116, "North Yorkshire"));
            _regions.Add(new Region(64, "Northamptonshire"));
            _regions.Add(new Region(65, "Northumberland"));
            _regions.Add(new Region(66, "Nottinghamshire"));
            _regions.Add(new Region(67, "Orkney Islands"));
            _regions.Add(new Region(68, "Oxfordshire"));
            _regions.Add(new Region(197, "Pembrokeshire"));
            _regions.Add(new Region(198, "Perth"));
            _regions.Add(new Region(71, "Powys"));
            _regions.Add(new Region(199, "Renfrewshire"));
            _regions.Add(new Region(200, "Rhondda"));
            _regions.Add(new Region(76, "Shetland Islands"));
            _regions.Add(new Region(77, "Shropshire"));
            _regions.Add(new Region(78, "Somerset"));
            _regions.Add(new Region(201, "South Ayrshire"));
            _regions.Add(new Region(79, "South Glamorgan"));
            _regions.Add(new Region(202, "South Lanarkshire"));
            _regions.Add(new Region(81, "South Yorkshire"));
            _regions.Add(new Region(82, "Staffordshire"));
            _regions.Add(new Region(203, "Stirling"));
            _regions.Add(new Region(117, "Strathclyde"));
            _regions.Add(new Region(84, "Suffolk"));
            _regions.Add(new Region(85, "Surrey"));
            _regions.Add(new Region(204, "Swansea"));
            _regions.Add(new Region(86, "Tayside"));
            _regions.Add(new Region(205, "Torfaen"));
            _regions.Add(new Region(87, "Tyne and Wear"));
            _regions.Add(new Region(215, "Ulster"));
            _regions.Add(new Region(214, "Ulster"));
            _regions.Add(new Region(90, "Warwickshire"));
            _regions.Add(new Region(206, "West Dunbartonshire"));
            _regions.Add(new Region(91, "West Glamorgan"));
            _regions.Add(new Region(207, "West Lothian"));
            _regions.Add(new Region(93, "West Midlands"));
            _regions.Add(new Region(94, "West Sussex"));
            _regions.Add(new Region(95, "West Yorkshire"));
            _regions.Add(new Region(96, "Western Isles"));
            _regions.Add(new Region(97, "Wiltshire"));
            _regions.Add(new Region(98, "Worcestershire"));
            _regions.Add(new Region(208, "Wrexham"));
		}

        public List<Region> AvailableRegions { get { return _regions; } }

		public int Count
		{
			get
			{
				return _regions.Count;
			}
		}

		public int CountyByName(string name)
		{

            var matches = _regions.Where(reg => reg.Name.Equals(name));
			if (!matches.Any())
			{
				// Not found
				return -1;
			}
			else
			{
                return matches.First().Id;
			}
		}

		public string CountyById(int id)
		{
            var matches = _regions.Where(reg => reg.Id == id);
			if (!matches.Any())
			{
				// Not found
				return null;
			}
			else
			{
                return matches.First().Name;
			}
		}
	}
}
