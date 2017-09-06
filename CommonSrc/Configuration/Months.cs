using System;
using System.Collections.Generic;
using System.Linq;

namespace IE.CommonSrc.Configuration
{
    public class Months
    {
        private Dictionary<int, string> _months = new Dictionary<int, string>();

        public Months()
        {
			_months.Add(168, "Aberdeen");
			_months.Add(169, "Aberdeenshire");
			_months.Add(2, "Anglesey");
			_months.Add(170, "Angus");
			_months.Add(171, "Argyll");
			_months.Add(101, "Avon");
			_months.Add(5, "Bedfordshire");
			_months.Add(6, "Berkshire");
			_months.Add(172, "Blaenau");
			_months.Add(102, "Borders");
			_months.Add(10, "Buckinghamshire");
			_months.Add(173, "Caerphilly");
			_months.Add(12, "Cambridgeshire");
			_months.Add(174, "Cardiff");
			_months.Add(175, "Carmarthenshire");
			_months.Add(103, "Central(Scotland)");
			_months.Add(176, "Ceredigion");
			_months.Add(104, "Channel Islands");
			_months.Add(16, "Cheshire");
			_months.Add(177, "Clackmannanshire");
			_months.Add(17, "Cleveland");
			_months.Add(18, "Clwyd");
			_months.Add(211, "Connaught");
			_months.Add(178, "Conwy");
			_months.Add(19, "Cornwall");
			_months.Add(20, "Cumbria");
			_months.Add(210, "Denbighshire");
			_months.Add(22, "Derbyshire");
			_months.Add(23, "Devon");
			_months.Add(24, "Dorset");
			_months.Add(25, "Dumfries & Galloway");
			_months.Add(179, "Dundee");
			_months.Add(27, "Durham");
			_months.Add(29, "Dyfed");
			_months.Add(180, "East Ayrshire");
			_months.Add(181, "East Dumbartonshire");
			_months.Add(182, "East Lothian");
			_months.Add(183, "East Renfrewshire");
			_months.Add(33, "East Sussex");
			_months.Add(184, "East Yorkshire");
			_months.Add(185, "Edinburgh");
			_months.Add(35, "Essex");
			_months.Add(209, "Falkirk");
			_months.Add(37, "Fife");
			_months.Add(187, "Flintshire");
			_months.Add(188, "Glasgow");
			_months.Add(40, "Gloucestershire");
			_months.Add(111, "Grampian");
			_months.Add(112, "Gwent");
			_months.Add(43, "Gwynedd");
			_months.Add(44, "Hampshire");
			_months.Add(45, "Herefordshire");
			_months.Add(46, "Hertfordshire");
			_months.Add(47, "Highlands");
			_months.Add(48, "Humberside");
			_months.Add(189, "Inverclyde");
			_months.Add(113, "Isle Of Man");
			_months.Add(49, "Isle of Wight");
			_months.Add(50, "Kent");
			_months.Add(51, "Lancashire");
			_months.Add(52, "Leicestershire");
			_months.Add(212, "Leinster");
			_months.Add(53, "Lincolnshire");
			_months.Add(41, "London");
			_months.Add(114, "Lothian");
			_months.Add(42, "Manchester");
			_months.Add(54, "Merseyside");
			_months.Add(190, "Merthyr");
			_months.Add(55, "Mid Glamorgan");
			_months.Add(191, "Monmouthshire");
			_months.Add(192, "Moray");
			_months.Add(213, "Munster");
			_months.Add(193, "Neath");
			_months.Add(194, "Newport");
			_months.Add(115, "Norfolk");
			_months.Add(195, "North Ayrshire");
			_months.Add(196, "North Lanarkshire");
			_months.Add(116, "North Yorkshire");
			_months.Add(64, "Northamptonshire");
			_months.Add(65, "Northumberland");
			_months.Add(66, "Nottinghamshire");
			_months.Add(67, "Orkney Islands");
			_months.Add(68, "Oxfordshire");
			_months.Add(197, "Pembrokeshire");
			_months.Add(198, "Perth");
			_months.Add(71, "Powys");
			_months.Add(199, "Renfrewshire");
			_months.Add(200, "Rhondda");
			_months.Add(76, "Shetland Islands");
			_months.Add(77, "Shropshire");
			_months.Add(78, "Somerset");
			_months.Add(201, "South Ayrshire");
			_months.Add(79, "South Glamorgan");
			_months.Add(202, "South Lanarkshire");
			_months.Add(81, "South Yorkshire");
			_months.Add(82, "Staffordshire");
			_months.Add(203, "Stirling");
			_months.Add(117, "Strathclyde");
			_months.Add(84, "Suffolk");
			_months.Add(85, "Surrey");
			_months.Add(204, "Swansea");
			_months.Add(86, "Tayside");
			_months.Add(205, "Torfaen");
			_months.Add(87, "Tyne and Wear");
			_months.Add(215, "Ulster");
			_months.Add(214, "Ulster");
			_months.Add(90, "Warwickshire");
			_months.Add(206, "West Dunbartonshire");
			_months.Add(91, "West Glamorgan");
			_months.Add(207, "West Lothian");
			_months.Add(93, "West Midlands");
			_months.Add(94, "West Sussex");
			_months.Add(95, "West Yorkshire");
			_months.Add(96, "Western Isles");
			_months.Add(97, "Wiltshire");
			_months.Add(98, "Worcestershire");
			_months.Add(208, "Wrexham");
		}

        public int Count 
        {
            get 
            {
                return _months.Count;
            }
        }

        public int CountyByName(string name) {

            var matches = _months.Where(month => month.Value.Equals(name));
            if ( !matches.Any()) 
            {
                // Not found
                return -1;
            }
            else
            {
                return matches.First().Key;
            }
        }

        public string CountyById(int id) {
            string outValue;

            if ( _months.TryGetValue(id, out outValue)) {
                return outValue;
            }
            return null;
        }
    }
}
