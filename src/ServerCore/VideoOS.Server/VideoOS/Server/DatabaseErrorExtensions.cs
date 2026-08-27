using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Server;

[GeneratedCode("GenerateSqlErrorExtensions", "0.0.0.0")]
public static class DatabaseErrorExtensions
{
	public static string FormatMessage(this DatabaseError databaseError)
	{
		IEnumerable<(int, string, string)> parameters;
		return databaseError.FormatMessage(out parameters);
	}

	public static string FormatMessage(this DatabaseError databaseError, out IEnumerable<(int Index, string Name, string Value)> parameters)
	{
		if (databaseError == null)
		{
			throw new ArgumentNullException("databaseError");
		}
		parameters = Enumerable.Empty<(int, string, string)>();
		switch (databaseError.Number)
		{
		case 60000:
			return VmoResources.VMO60000;
		case 60001:
		{
			string text233 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text233) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60001, text233);
		}
		case 60002:
		{
			string text232 = databaseError.ExtractString("IDRole");
			parameters = new(int, string, string)[1] { (0, "IDRole", text232) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60002, text232);
		}
		case 60003:
		{
			string text231 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text231) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60003, text231);
		}
		case 60004:
		{
			string text230 = databaseError.ExtractString("IDRole");
			parameters = new(int, string, string)[1] { (0, "IDRole", text230) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60004, text230);
		}
		case 60005:
			return VmoResources.VMO60005;
		case 60006:
		{
			string text229 = databaseError.ExtractString("IDRole");
			parameters = new(int, string, string)[1] { (0, "IDRole", text229) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60006, text229);
		}
		case 60007:
		{
			string text227 = databaseError.ExtractString("RoleName");
			string text228 = databaseError.ExtractString("MemberName");
			parameters = new(int, string, string)[2]
			{
				(0, "RoleName", text227),
				(1, "MemberName", text228)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60007, text227, text228);
		}
		case 60008:
			return VmoResources.VMO60008;
		case 60009:
		{
			string text226 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text226) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60009, text226);
		}
		case 60010:
			return VmoResources.VMO60010;
		case 60011:
		{
			string text225 = databaseError.ExtractString("MemberSid");
			parameters = new(int, string, string)[1] { (0, "MemberSid", text225) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60011, text225);
		}
		case 60012:
		{
			string text224 = databaseError.ExtractString("IDObject");
			parameters = new(int, string, string)[1] { (0, "IDObject", text224) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60012, text224);
		}
		case 60013:
		{
			string text223 = databaseError.ExtractString("IDAction");
			parameters = new(int, string, string)[1] { (0, "IDAction", text223) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60013, text223);
		}
		case 60014:
		{
			string text222 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text222) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60014, text222);
		}
		case 60015:
		{
			string text221 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text221) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60015, text221);
		}
		case 60016:
		{
			string text220 = databaseError.ExtractString("IDObject");
			parameters = new(int, string, string)[1] { (0, "IDObject", text220) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60016, text220);
		}
		case 60017:
		{
			string text219 = databaseError.ExtractString("IDClass");
			parameters = new(int, string, string)[1] { (0, "IDClass", text219) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60017, text219);
		}
		case 60018:
		{
			string text218 = databaseError.ExtractString("IDParentObject");
			parameters = new(int, string, string)[1] { (0, "IDParentObject", text218) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60018, text218);
		}
		case 60019:
		{
			string text216 = databaseError.ExtractString("IDParentObject");
			string text217 = databaseError.ExtractString("IDObject");
			parameters = new(int, string, string)[2]
			{
				(0, "IDParentObject", text216),
				(1, "IDObject", text217)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60019, text216, text217);
		}
		case 60020:
		{
			string text215 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text215) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60020, text215);
		}
		case 60021:
		{
			string text214 = databaseError.ExtractString("IDObject");
			parameters = new(int, string, string)[1] { (0, "IDObject", text214) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60021, text214);
		}
		case 60022:
			return VmoResources.VMO60022;
		case 60023:
		{
			string text213 = databaseError.ExtractString("IDClass");
			parameters = new(int, string, string)[1] { (0, "IDClass", text213) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60023, text213);
		}
		case 60024:
		{
			string text210 = databaseError.ExtractString("IDObject");
			string text211 = databaseError.ExtractString("IDAction");
			string text212 = databaseError.ExtractString("IDUser");
			parameters = new(int, string, string)[3]
			{
				(0, "IDObject", text210),
				(1, "IDAction", text211),
				(2, "IDUser", text212)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60024, text210, text211, text212);
		}
		case 60025:
		{
			string text208 = databaseError.ExtractString("name");
			string text209 = databaseError.ExtractString("recordingServerName");
			parameters = new(int, string, string)[2]
			{
				(0, "name", text208),
				(1, "recordingServerName", text209)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60025, text208, text209);
		}
		case 60026:
		{
			string text206 = databaseError.ExtractString("Name");
			string text207 = databaseError.ExtractString("recordingServerName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text206),
				(1, "recordingServerName", text207)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60026, text206, text207);
		}
		case 60027:
			return VmoResources.VMO60027;
		case 60028:
			return VmoResources.VMO60028;
		case 60029:
			return VmoResources.VMO60029;
		case 60030:
		{
			string text205 = databaseError.ExtractString("IDParentGroup");
			parameters = new(int, string, string)[1] { (0, "IDParentGroup", text205) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60030, text205);
		}
		case 60031:
		{
			string text204 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text204) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60031, text204);
		}
		case 60032:
		{
			string text203 = databaseError.ExtractString("IDFailoverGroup");
			parameters = new(int, string, string)[1] { (0, "IDFailoverGroup", text203) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60032, text203);
		}
		case 60033:
		{
			string text202 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text202) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60033, text202);
		}
		case 60034:
		{
			string text201 = databaseError.ExtractString("IDFailoverGroup");
			parameters = new(int, string, string)[1] { (0, "IDFailoverGroup", text201) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60034, text201);
		}
		case 60035:
			return VmoResources.VMO60035;
		case 60036:
			return VmoResources.VMO60036;
		case 60037:
		{
			string text199 = databaseError.ExtractString("Name");
			string text200 = databaseError.ExtractString("groupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text199),
				(1, "groupName", text200)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60037, text199, text200);
		}
		case 60038:
		{
			string text198 = databaseError.ExtractString("IDFailoverGroup");
			parameters = new(int, string, string)[1] { (0, "IDFailoverGroup", text198) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60038, text198);
		}
		case 60039:
		{
			string text197 = databaseError.ExtractString("IDFailoverServer");
			parameters = new(int, string, string)[1] { (0, "IDFailoverServer", text197) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60039, text197);
		}
		case 60040:
		{
			string text195 = databaseError.ExtractString("Name");
			string text196 = databaseError.ExtractString("groupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text195),
				(1, "groupName", text196)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60040, text195, text196);
		}
		case 60041:
		{
			string text194 = databaseError.ExtractString("IDTargetGroup");
			parameters = new(int, string, string)[1] { (0, "IDTargetGroup", text194) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60041, text194);
		}
		case 60042:
		{
			string text192 = databaseError.ExtractString("targetGroupName");
			string text193 = databaseError.ExtractString("failoverServerName");
			parameters = new(int, string, string)[2]
			{
				(0, "targetGroupName", text192),
				(1, "failoverServerName", text193)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60042, text192, text193);
		}
		case 60043:
			return VmoResources.VMO60043;
		case 60044:
		{
			string text191 = databaseError.ExtractString("IDEventType");
			parameters = new(int, string, string)[1] { (0, "IDEventType", text191) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60044, text191);
		}
		case 60045:
			return VmoResources.VMO60045;
		case 60046:
			return VmoResources.VMO60046;
		case 60047:
			return VmoResources.VMO60047;
		case 60048:
		{
			string text190 = databaseError.ExtractString("IDFailoverServer");
			parameters = new(int, string, string)[1] { (0, "IDFailoverServer", text190) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60048, text190);
		}
		case 60049:
		{
			string text188 = databaseError.ExtractString("Name");
			string text189 = databaseError.ExtractString("recordingServerName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text188),
				(1, "recordingServerName", text189)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60049, text188, text189);
		}
		case 60050:
		{
			string text186 = databaseError.ExtractString("Name");
			string text187 = databaseError.ExtractString("recordingServerName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text186),
				(1, "recordingServerName", text187)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60050, text186, text187);
		}
		case 60051:
			return VmoResources.VMO60051;
		case 60052:
			return VmoResources.VMO60052;
		case 60053:
			return VmoResources.VMO60053;
		case 60054:
			return VmoResources.VMO60054;
		case 60055:
		{
			string text184 = databaseError.ExtractString("Name");
			string text185 = databaseError.ExtractString("groupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text184),
				(1, "groupName", text185)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60055, text184, text185);
		}
		case 60056:
			return VmoResources.VMO60056;
		case 60057:
			return VmoResources.VMO60057;
		case 60058:
			return VmoResources.VMO60058;
		case 60059:
			return VmoResources.VMO60059;
		case 60060:
			return VmoResources.VMO60060;
		case 60061:
		{
			string text183 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text183) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60061, text183);
		}
		case 60062:
			return VmoResources.VMO60062;
		case 60063:
			return VmoResources.VMO60063;
		case 60064:
			return VmoResources.VMO60064;
		case 60065:
			return VmoResources.VMO60065;
		case 60066:
			return VmoResources.VMO60066;
		case 60067:
			return VmoResources.VMO60067;
		case 60068:
			return VmoResources.VMO60068;
		case 60069:
		{
			string text181 = databaseError.ExtractString("IDRecorderPlugin");
			string text182 = databaseError.ExtractString("IDDevice");
			parameters = new(int, string, string)[2]
			{
				(0, "IDRecorderPlugin", text181),
				(1, "IDDevice", text182)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60069, text181, text182);
		}
		case 60070:
		{
			string text179 = databaseError.ExtractString("IDRecorderPlugin");
			string text180 = databaseError.ExtractString("IDDeviceTemplate");
			parameters = new(int, string, string)[2]
			{
				(0, "IDRecorderPlugin", text179),
				(1, "IDDeviceTemplate", text180)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60070, text179, text180);
		}
		case 60071:
			return VmoResources.VMO60071;
		case 60072:
			return VmoResources.VMO60072;
		case 60073:
			return VmoResources.VMO60073;
		case 60074:
			return VmoResources.VMO60074;
		case 60075:
			return VmoResources.VMO60075;
		case 60076:
			return VmoResources.VMO60076;
		case 60077:
			return VmoResources.VMO60077;
		case 60078:
			return VmoResources.VMO60078;
		case 60079:
			return VmoResources.VMO60079;
		case 60080:
			return VmoResources.VMO60080;
		case 60081:
			return VmoResources.VMO60081;
		case 60082:
			return VmoResources.VMO60082;
		case 60083:
			return VmoResources.VMO60083;
		case 60084:
			return VmoResources.VMO60084;
		case 60085:
			return VmoResources.VMO60085;
		case 60086:
			return VmoResources.VMO60086;
		case 60087:
		{
			string text177 = databaseError.ExtractString("Name");
			string text178 = databaseError.ExtractString("groupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text177),
				(1, "groupName", text178)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60087, text177, text178);
		}
		case 60088:
			return VmoResources.VMO60088;
		case 60089:
			return VmoResources.VMO60089;
		case 60090:
			return VmoResources.VMO60090;
		case 60091:
			return VmoResources.VMO60091;
		case 60092:
			return VmoResources.VMO60092;
		case 60093:
			return VmoResources.VMO60093;
		case 60094:
			return VmoResources.VMO60094;
		case 60095:
			return VmoResources.VMO60095;
		case 60096:
			return VmoResources.VMO60096;
		case 60097:
			return VmoResources.VMO60097;
		case 60098:
			return VmoResources.VMO60098;
		case 60099:
			return VmoResources.VMO60099;
		case 60100:
			return VmoResources.VMO60100;
		case 60101:
			return VmoResources.VMO60101;
		case 60102:
		{
			string text176 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text176) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60102, text176);
		}
		case 60103:
		{
			string text175 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text175) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60103, text175);
		}
		case 60104:
		{
			string text174 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text174) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60104, text174);
		}
		case 60105:
			return VmoResources.VMO60105;
		case 60106:
			return VmoResources.VMO60106;
		case 60107:
			return VmoResources.VMO60107;
		case 60108:
		{
			string text172 = databaseError.ExtractString("Name");
			string text173 = databaseError.ExtractString("GroupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text172),
				(1, "GroupName", text173)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60108, text172, text173);
		}
		case 60109:
			return VmoResources.VMO60109;
		case 60110:
			return VmoResources.VMO60110;
		case 60111:
		{
			string text170 = databaseError.ExtractString("Name");
			string text171 = databaseError.ExtractString("GroupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text170),
				(1, "GroupName", text171)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60111, text170, text171);
		}
		case 60112:
			return VmoResources.VMO60112;
		case 60113:
			return VmoResources.VMO60113;
		case 60114:
			return VmoResources.VMO60114;
		case 60115:
			return VmoResources.VMO60115;
		case 60116:
		{
			string text168 = databaseError.ExtractString("Name");
			string text169 = databaseError.ExtractString("GroupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text168),
				(1, "GroupName", text169)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60116, text168, text169);
		}
		case 60117:
			return VmoResources.VMO60117;
		case 60118:
			return VmoResources.VMO60118;
		case 60119:
			return VmoResources.VMO60119;
		case 60120:
			return VmoResources.VMO60120;
		case 60121:
			return VmoResources.VMO60121;
		case 60122:
			return VmoResources.VMO60122;
		case 60123:
		{
			string text166 = databaseError.ExtractString("Name");
			string text167 = databaseError.ExtractString("GroupName");
			parameters = new(int, string, string)[2]
			{
				(0, "Name", text166),
				(1, "GroupName", text167)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60123, text166, text167);
		}
		case 60124:
			return VmoResources.VMO60124;
		case 60125:
			return VmoResources.VMO60125;
		case 60126:
			return VmoResources.VMO60126;
		case 60127:
			return VmoResources.VMO60127;
		case 60128:
			return VmoResources.VMO60128;
		case 60129:
			return VmoResources.VMO60129;
		case 60130:
			return VmoResources.VMO60130;
		case 60131:
			return VmoResources.VMO60131;
		case 60132:
			return VmoResources.VMO60132;
		case 60133:
			return VmoResources.VMO60133;
		case 60134:
			return VmoResources.VMO60134;
		case 60135:
			return VmoResources.VMO60135;
		case 60136:
			return VmoResources.VMO60136;
		case 60137:
			return VmoResources.VMO60137;
		case 60138:
			return VmoResources.VMO60138;
		case 60139:
			return VmoResources.VMO60139;
		case 60140:
			return VmoResources.VMO60140;
		case 60141:
			return VmoResources.VMO60141;
		case 60142:
			return VmoResources.VMO60142;
		case 60143:
			return VmoResources.VMO60143;
		case 60144:
			return VmoResources.VMO60144;
		case 60145:
			return VmoResources.VMO60145;
		case 60146:
			return VmoResources.VMO60146;
		case 60147:
			return VmoResources.VMO60147;
		case 60148:
			return VmoResources.VMO60148;
		case 60149:
			return VmoResources.VMO60149;
		case 60150:
			return VmoResources.VMO60150;
		case 60151:
			return VmoResources.VMO60151;
		case 60152:
			return VmoResources.VMO60152;
		case 60153:
			return VmoResources.VMO60153;
		case 60154:
			return VmoResources.VMO60154;
		case 60155:
		{
			string text165 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text165) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60155, text165);
		}
		case 60156:
		{
			string text164 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text164) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60156, text164);
		}
		case 60157:
			return VmoResources.VMO60157;
		case 60158:
			return VmoResources.VMO60158;
		case 60159:
			return VmoResources.VMO60159;
		case 60160:
		{
			string text163 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text163) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60160, text163);
		}
		case 60161:
			return VmoResources.VMO60161;
		case 60162:
		{
			string text162 = databaseError.ExtractString("maxItems");
			parameters = new(int, string, string)[1] { (0, "maxItems", text162) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60162, text162);
		}
		case 60163:
			return VmoResources.VMO60163;
		case 60164:
			return VmoResources.VMO60164;
		case 60165:
			return VmoResources.VMO60165;
		case 60166:
			return VmoResources.VMO60166;
		case 60167:
			return VmoResources.VMO60167;
		case 60168:
			return VmoResources.VMO60168;
		case 60169:
			return VmoResources.VMO60169;
		case 60174:
			return VmoResources.VMO60174;
		case 60175:
			return VmoResources.VMO60175;
		case 60176:
			return VmoResources.VMO60176;
		case 60177:
			return VmoResources.VMO60177;
		case 60178:
			return VmoResources.VMO60178;
		case 60179:
			return VmoResources.VMO60179;
		case 60180:
			return VmoResources.VMO60180;
		case 60181:
			return VmoResources.VMO60181;
		case 60182:
			return VmoResources.VMO60182;
		case 60183:
			return VmoResources.VMO60183;
		case 60184:
			return VmoResources.VMO60184;
		case 60185:
			return VmoResources.VMO60185;
		case 60186:
			return VmoResources.VMO60186;
		case 60187:
			return VmoResources.VMO60187;
		case 60188:
			return VmoResources.VMO60188;
		case 60189:
			return VmoResources.VMO60189;
		case 60190:
			return VmoResources.VMO60190;
		case 60191:
			return VmoResources.VMO60191;
		case 60192:
			return VmoResources.VMO60192;
		case 60193:
		{
			string text161 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text161) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60193, text161);
		}
		case 60194:
		{
			string text160 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text160) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60194, text160);
		}
		case 60195:
		{
			string text159 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text159) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60195, text159);
		}
		case 60196:
		{
			string text158 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text158) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60196, text158);
		}
		case 60197:
			return VmoResources.VMO60197;
		case 60198:
			return VmoResources.VMO60198;
		case 60199:
			return VmoResources.VMO60199;
		case 60200:
		{
			string text157 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text157) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60200, text157);
		}
		case 60201:
			return VmoResources.VMO60201;
		case 60202:
		{
			string text156 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text156) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60202, text156);
		}
		case 60203:
			return VmoResources.VMO60203;
		case 60204:
		{
			string text154 = databaseError.ExtractString("name");
			string text155 = databaseError.ExtractString("videoWallName");
			parameters = new(int, string, string)[2]
			{
				(0, "name", text154),
				(1, "videoWallName", text155)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60204, text154, text155);
		}
		case 60205:
			return VmoResources.VMO60205;
		case 60206:
		{
			string text152 = databaseError.ExtractString("name");
			string text153 = databaseError.ExtractString("videoWallName");
			parameters = new(int, string, string)[2]
			{
				(0, "name", text152),
				(1, "videoWallName", text153)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60206, text152, text153);
		}
		case 60207:
			return VmoResources.VMO60207;
		case 60208:
			return VmoResources.VMO60208;
		case 60209:
			return VmoResources.VMO60209;
		case 60210:
		{
			string text150 = databaseError.ExtractString("name");
			string text151 = databaseError.ExtractString("videoWallName");
			parameters = new(int, string, string)[2]
			{
				(0, "name", text150),
				(1, "videoWallName", text151)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60210, text150, text151);
		}
		case 60211:
			return VmoResources.VMO60211;
		case 60212:
		{
			string text148 = databaseError.ExtractString("name");
			string text149 = databaseError.ExtractString("videoWallName");
			parameters = new(int, string, string)[2]
			{
				(0, "name", text148),
				(1, "videoWallName", text149)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60212, text148, text149);
		}
		case 60213:
			return VmoResources.VMO60213;
		case 60214:
		{
			string text147 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text147) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60214, text147);
		}
		case 60215:
			return VmoResources.VMO60215;
		case 60216:
		{
			string text146 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text146) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60216, text146);
		}
		case 60217:
			return VmoResources.VMO60217;
		case 60218:
		{
			string text145 = databaseError.ExtractString("Id");
			parameters = new(int, string, string)[1] { (0, "Id", text145) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60218, text145);
		}
		case 60219:
		{
			string text144 = databaseError.ExtractString("Id");
			parameters = new(int, string, string)[1] { (0, "Id", text144) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60219, text144);
		}
		case 60220:
			return VmoResources.VMO60220;
		case 60221:
			return VmoResources.VMO60221;
		case 60222:
		{
			string text143 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text143) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60222, text143);
		}
		case 60223:
			return VmoResources.VMO60223;
		case 60224:
		{
			string text142 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text142) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60224, text142);
		}
		case 60225:
			return VmoResources.VMO60225;
		case 60226:
			return VmoResources.VMO60226;
		case 60227:
			return VmoResources.VMO60227;
		case 60228:
			return VmoResources.VMO60228;
		case 60229:
			return VmoResources.VMO60229;
		case 60230:
			return VmoResources.VMO60230;
		case 60231:
			return VmoResources.VMO60231;
		case 60232:
			return VmoResources.VMO60232;
		case 60233:
			return VmoResources.VMO60233;
		case 60234:
			return VmoResources.VMO60234;
		case 60235:
			return VmoResources.VMO60235;
		case 60236:
			return VmoResources.VMO60236;
		case 60237:
			return VmoResources.VMO60237;
		case 60238:
			return VmoResources.VMO60238;
		case 60239:
			return VmoResources.VMO60239;
		case 60240:
			return VmoResources.VMO60240;
		case 60241:
			return VmoResources.VMO60241;
		case 60242:
			return VmoResources.VMO60242;
		case 60243:
			return VmoResources.VMO60243;
		case 60244:
			return VmoResources.VMO60244;
		case 60245:
			return VmoResources.VMO60245;
		case 60246:
			return VmoResources.VMO60246;
		case 60247:
			return VmoResources.VMO60247;
		case 60248:
			return VmoResources.VMO60248;
		case 60249:
			return VmoResources.VMO60249;
		case 60250:
			return VmoResources.VMO60250;
		case 60251:
			return VmoResources.VMO60251;
		case 60252:
			return VmoResources.VMO60252;
		case 60253:
		{
			string text141 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text141) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60253, text141);
		}
		case 60254:
			return VmoResources.VMO60254;
		case 60255:
			return VmoResources.VMO60255;
		case 60256:
			return VmoResources.VMO60256;
		case 60257:
			return VmoResources.VMO60257;
		case 60258:
			return VmoResources.VMO60258;
		case 60259:
			return VmoResources.VMO60259;
		case 60260:
			return VmoResources.VMO60260;
		case 60261:
			return VmoResources.VMO60261;
		case 60262:
			return VmoResources.VMO60262;
		case 60263:
			return VmoResources.VMO60263;
		case 60264:
			return VmoResources.VMO60264;
		case 60265:
			return VmoResources.VMO60265;
		case 60266:
			return VmoResources.VMO60266;
		case 60267:
			return VmoResources.VMO60267;
		case 60268:
			return VmoResources.VMO60268;
		case 60269:
			return VmoResources.VMO60269;
		case 60270:
			return VmoResources.VMO60270;
		case 60271:
			return VmoResources.VMO60271;
		case 60272:
			return VmoResources.VMO60272;
		case 60273:
			return VmoResources.VMO60273;
		case 60274:
			return VmoResources.VMO60274;
		case 60275:
			return VmoResources.VMO60275;
		case 60276:
			return VmoResources.VMO60276;
		case 60277:
			return VmoResources.VMO60277;
		case 60278:
			return VmoResources.VMO60278;
		case 60279:
			return VmoResources.VMO60279;
		case 60280:
			return VmoResources.VMO60280;
		case 60281:
			return VmoResources.VMO60281;
		case 60282:
			return VmoResources.VMO60282;
		case 60283:
			return VmoResources.VMO60283;
		case 60284:
		{
			string text140 = databaseError.ExtractString("Message");
			parameters = new(int, string, string)[1] { (0, "Message", text140) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60284, text140);
		}
		case 60285:
			return VmoResources.VMO60285;
		case 60286:
		{
			string text138 = databaseError.ExtractString("ProductName");
			string text139 = databaseError.ExtractString("ProductVersion");
			parameters = new(int, string, string)[2]
			{
				(0, "ProductName", text138),
				(1, "ProductVersion", text139)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60286, text138, text139);
		}
		case 60287:
			return VmoResources.VMO60287;
		case 60288:
		{
			string text136 = databaseError.ExtractString("ProductName");
			string text137 = databaseError.ExtractString("ProductVersion");
			parameters = new(int, string, string)[2]
			{
				(0, "ProductName", text136),
				(1, "ProductVersion", text137)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60288, text136, text137);
		}
		case 60289:
			return VmoResources.VMO60289;
		case 60290:
		{
			string text134 = databaseError.ExtractString("ProductName");
			string text135 = databaseError.ExtractString("ProductVersion");
			parameters = new(int, string, string)[2]
			{
				(0, "ProductName", text134),
				(1, "ProductVersion", text135)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60290, text134, text135);
		}
		case 60291:
			return VmoResources.VMO60291;
		case 60292:
		{
			string text132 = databaseError.ExtractString("ProductName");
			string text133 = databaseError.ExtractString("ProductVersion");
			parameters = new(int, string, string)[2]
			{
				(0, "ProductName", text132),
				(1, "ProductVersion", text133)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60292, text132, text133);
		}
		case 60293:
			return VmoResources.VMO60293;
		case 60294:
			return VmoResources.VMO60294;
		case 60295:
			return VmoResources.VMO60295;
		case 60296:
			return VmoResources.VMO60296;
		case 60297:
			return VmoResources.VMO60297;
		case 60298:
			return VmoResources.VMO60298;
		case 60299:
			return VmoResources.VMO60299;
		case 60300:
			return VmoResources.VMO60300;
		case 60301:
			return VmoResources.VMO60301;
		case 60302:
			return VmoResources.VMO60302;
		case 60303:
			return VmoResources.VMO60303;
		case 60304:
			return VmoResources.VMO60304;
		case 60305:
			return VmoResources.VMO60305;
		case 60306:
			return VmoResources.VMO60306;
		case 60307:
			return VmoResources.VMO60307;
		case 60308:
			return VmoResources.VMO60308;
		case 60309:
			return VmoResources.VMO60309;
		case 60310:
			return VmoResources.VMO60310;
		case 60311:
		{
			string text131 = databaseError.ExtractString("FunctionName");
			parameters = new(int, string, string)[1] { (0, "FunctionName", text131) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60311, text131);
		}
		case 60312:
			return VmoResources.VMO60312;
		case 60313:
			return VmoResources.VMO60313;
		case 60314:
			return VmoResources.VMO60314;
		case 60315:
			return VmoResources.VMO60315;
		case 60316:
			return VmoResources.VMO60316;
		case 60317:
			return VmoResources.VMO60317;
		case 60318:
			return VmoResources.VMO60318;
		case 60319:
			return VmoResources.VMO60319;
		case 60320:
		{
			string text130 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text130) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60320, text130);
		}
		case 60321:
			return VmoResources.VMO60321;
		case 60322:
			return VmoResources.VMO60322;
		case 60323:
			return VmoResources.VMO60323;
		case 60324:
			return VmoResources.VMO60324;
		case 60325:
			return VmoResources.VMO60325;
		case 60326:
		{
			string text129 = databaseError.ExtractString("HardwareName");
			parameters = new(int, string, string)[1] { (0, "HardwareName", text129) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60326, text129);
		}
		case 60327:
			return VmoResources.VMO60327;
		case 60328:
			return VmoResources.VMO60328;
		case 60329:
			return VmoResources.VMO60329;
		case 60330:
			return VmoResources.VMO60330;
		case 60331:
			return VmoResources.VMO60331;
		case 60332:
			return VmoResources.VMO60332;
		case 60333:
			return VmoResources.VMO60333;
		case 60334:
			return VmoResources.VMO60334;
		case 60335:
		{
			string text128 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text128) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60335, text128);
		}
		case 60336:
			return VmoResources.VMO60336;
		case 60337:
			return VmoResources.VMO60337;
		case 60338:
			return VmoResources.VMO60338;
		case 60339:
		{
			string text125 = databaseError.ExtractString("IDHardware");
			string text126 = databaseError.ExtractString("DeviceType");
			string text127 = databaseError.ExtractString("DeviceIndex");
			parameters = new(int, string, string)[3]
			{
				(0, "IDHardware", text125),
				(1, "DeviceType", text126),
				(2, "DeviceIndex", text127)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60339, text125, text126, text127);
		}
		case 60340:
		{
			string text123 = databaseError.ExtractString("LoginProviderId");
			string text124 = databaseError.ExtractString("ClaimName");
			parameters = new(int, string, string)[2]
			{
				(0, "LoginProviderId", text123),
				(1, "ClaimName", text124)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60340, text123, text124);
		}
		case 60341:
		{
			string text121 = databaseError.ExtractString("LoginProviderId");
			string text122 = databaseError.ExtractString("ClaimName");
			parameters = new(int, string, string)[2]
			{
				(0, "LoginProviderId", text121),
				(1, "ClaimName", text122)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60341, text121, text122);
		}
		case 60342:
			return VmoResources.VMO60342;
		case 60343:
			return VmoResources.VMO60343;
		case 60344:
			return VmoResources.VMO60344;
		case 60345:
			return VmoResources.VMO60345;
		case 60346:
			return VmoResources.VMO60346;
		case 60347:
			return VmoResources.VMO60347;
		case 60348:
		{
			string text120 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text120) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60348, text120);
		}
		case 60349:
		{
			string text119 = databaseError.ExtractString("DisplayName");
			parameters = new(int, string, string)[1] { (0, "DisplayName", text119) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60349, text119);
		}
		case 60350:
			return VmoResources.VMO60350;
		case 60351:
		{
			string text118 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text118) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60351, text118);
		}
		case 60352:
			return VmoResources.VMO60352;
		case 60353:
		{
			string text117 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text117) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60353, text117);
		}
		case 60354:
			return VmoResources.VMO60354;
		case 60355:
			return VmoResources.VMO60355;
		case 60356:
			return VmoResources.VMO60356;
		case 60357:
			return VmoResources.VMO60357;
		case 60358:
			return VmoResources.VMO60358;
		case 60359:
			return VmoResources.VMO60359;
		case 60360:
			return VmoResources.VMO60360;
		case 60361:
			return VmoResources.VMO60361;
		case 60362:
			return VmoResources.VMO60362;
		case 60363:
			return VmoResources.VMO60363;
		case 60364:
			return VmoResources.VMO60364;
		case 60365:
			return VmoResources.VMO60365;
		case 60366:
		{
			string text116 = databaseError.ExtractString("DisplayName");
			parameters = new(int, string, string)[1] { (0, "DisplayName", text116) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60366, text116);
		}
		case 60367:
			return VmoResources.VMO60367;
		case 60368:
			return VmoResources.VMO60368;
		case 60369:
			return VmoResources.VMO60369;
		case 60370:
			return VmoResources.VMO60370;
		case 60371:
			return VmoResources.VMO60371;
		case 60372:
			return VmoResources.VMO60372;
		case 60373:
		{
			string text115 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text115) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60373, text115);
		}
		case 60374:
			return VmoResources.VMO60374;
		case 60375:
			return VmoResources.VMO60375;
		case 60376:
			return VmoResources.VMO60376;
		case 60377:
			return VmoResources.VMO60377;
		case 60378:
			return VmoResources.VMO60378;
		case 60379:
			return VmoResources.VMO60379;
		case 60380:
			return VmoResources.VMO60380;
		case 60381:
			return VmoResources.VMO60381;
		case 60382:
			return VmoResources.VMO60382;
		case 60383:
			return VmoResources.VMO60383;
		case 60384:
			return VmoResources.VMO60384;
		case 60385:
			return VmoResources.VMO60385;
		case 60386:
			return VmoResources.VMO60386;
		case 60387:
			return VmoResources.VMO60387;
		case 60388:
			return VmoResources.VMO60388;
		case 60389:
			return VmoResources.VMO60389;
		case 60390:
			return VmoResources.VMO60390;
		case 60391:
			return VmoResources.VMO60391;
		case 60392:
			return VmoResources.VMO60392;
		case 60393:
			return VmoResources.VMO60393;
		case 60394:
			return VmoResources.VMO60394;
		case 60395:
			return VmoResources.VMO60395;
		case 60396:
			return VmoResources.VMO60396;
		case 60397:
			return VmoResources.VMO60397;
		case 60398:
			return VmoResources.VMO60398;
		case 60399:
			return VmoResources.VMO60399;
		case 60400:
			return VmoResources.VMO60400;
		case 60401:
			return VmoResources.VMO60401;
		case 60402:
			return VmoResources.VMO60402;
		case 60403:
			return VmoResources.VMO60403;
		case 60404:
			return VmoResources.VMO60404;
		case 60405:
			return VmoResources.VMO60405;
		case 60406:
		{
			string text113 = databaseError.ExtractString("FromSite");
			string text114 = databaseError.ExtractString("ToSite");
			parameters = new(int, string, string)[2]
			{
				(0, "FromSite", text113),
				(1, "ToSite", text114)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60406, text113, text114);
		}
		case 60407:
			return VmoResources.VMO60407;
		case 60408:
			return VmoResources.VMO60408;
		case 60409:
			return VmoResources.VMO60409;
		case 60410:
			return VmoResources.VMO60410;
		case 60411:
			return VmoResources.VMO60411;
		case 60412:
			return VmoResources.VMO60412;
		case 60413:
			return VmoResources.VMO60413;
		case 60414:
			return VmoResources.VMO60414;
		case 60415:
			return VmoResources.VMO60415;
		case 60416:
			return VmoResources.VMO60416;
		case 60417:
			return VmoResources.VMO60417;
		case 60451:
		{
			string text112 = databaseError.ExtractString("Id");
			parameters = new(int, string, string)[1] { (0, "Id", text112) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60451, text112);
		}
		case 60452:
			return VmoResources.VMO60452;
		case 60453:
			return VmoResources.VMO60453;
		case 60454:
			return VmoResources.VMO60454;
		case 60455:
			return VmoResources.VMO60455;
		case 60456:
			return VmoResources.VMO60456;
		case 60460:
			return VmoResources.VMO60460;
		case 60461:
			return VmoResources.VMO60461;
		case 60464:
			return VmoResources.VMO60464;
		case 60465:
			return VmoResources.VMO60465;
		case 60470:
			return VmoResources.VMO60470;
		case 60471:
			return VmoResources.VMO60471;
		case 60472:
			return VmoResources.VMO60472;
		case 60473:
			return VmoResources.VMO60473;
		case 60474:
			return VmoResources.VMO60474;
		case 60475:
			return VmoResources.VMO60475;
		case 60476:
			return VmoResources.VMO60476;
		case 60477:
			return VmoResources.VMO60477;
		case 60480:
			return VmoResources.VMO60480;
		case 60481:
		{
			string text110 = databaseError.ExtractString("Id");
			string text111 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[2]
			{
				(0, "Id", text110),
				(1, "Name", text111)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60481, text110, text111);
		}
		case 60482:
		{
			string text109 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text109) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60482, text109);
		}
		case 60483:
			return VmoResources.VMO60483;
		case 60484:
		{
			string text108 = databaseError.ExtractString("IDEventType");
			parameters = new(int, string, string)[1] { (0, "IDEventType", text108) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60484, text108);
		}
		case 60501:
			return VmoResources.VMO60501;
		case 60502:
			return VmoResources.VMO60502;
		case 60510:
		{
			string text105 = databaseError.ExtractString("address");
			string text106 = databaseError.ExtractString("name");
			string text107 = databaseError.ExtractString("exceptionMessage");
			parameters = new(int, string, string)[3]
			{
				(0, "address", text105),
				(1, "name", text106),
				(2, "exceptionMessage", text107)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60510, text105, text106, text107);
		}
		case 60511:
		{
			string text103 = databaseError.ExtractString("address");
			string text104 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[2]
			{
				(0, "address", text103),
				(1, "name", text104)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60511, text103, text104);
		}
		case 60512:
			return VmoResources.VMO60512;
		case 60513:
			return VmoResources.VMO60513;
		case 60514:
			return VmoResources.VMO60514;
		case 60515:
			return VmoResources.VMO60515;
		case 60516:
			return VmoResources.VMO60516;
		case 60517:
			return VmoResources.VMO60517;
		case 60518:
			return VmoResources.VMO60518;
		case 60519:
			return VmoResources.VMO60519;
		case 60520:
		{
			string text102 = databaseError.ExtractString("error");
			parameters = new(int, string, string)[1] { (0, "error", text102) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60520, text102);
		}
		case 60521:
			return VmoResources.VMO60521;
		case 60530:
			return VmoResources.VMO60530;
		case 60531:
			return VmoResources.VMO60531;
		case 60601:
		{
			string text101 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text101) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60601, text101);
		}
		case 60602:
		{
			string text100 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text100) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60602, text100);
		}
		case 60603:
		{
			string text99 = databaseError.ExtractString("pkid");
			parameters = new(int, string, string)[1] { (0, "pkid", text99) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60603, text99);
		}
		case 60604:
		{
			string text98 = databaseError.ExtractString("pkid");
			parameters = new(int, string, string)[1] { (0, "pkid", text98) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60604, text98);
		}
		case 60605:
		{
			string text97 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text97) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60605, text97);
		}
		case 60611:
		{
			string text96 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text96) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60611, text96);
		}
		case 60612:
		{
			string text95 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text95) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60612, text95);
		}
		case 60613:
		{
			string text94 = databaseError.ExtractString("pkid");
			parameters = new(int, string, string)[1] { (0, "pkid", text94) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60613, text94);
		}
		case 60614:
		{
			string text93 = databaseError.ExtractString("pkid");
			parameters = new(int, string, string)[1] { (0, "pkid", text93) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60614, text93);
		}
		case 60615:
		{
			string text92 = databaseError.ExtractString("Id");
			parameters = new(int, string, string)[1] { (0, "Id", text92) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60615, text92);
		}
		case 60616:
			return VmoResources.VMO60616;
		case 60630:
			return VmoResources.VMO60630;
		case 60631:
			return VmoResources.VMO60631;
		case 60632:
			return VmoResources.VMO60632;
		case 60633:
			return VmoResources.VMO60633;
		case 60634:
			return VmoResources.VMO60634;
		case 60635:
			return VmoResources.VMO60635;
		case 60636:
			return VmoResources.VMO60636;
		case 60637:
			return VmoResources.VMO60637;
		case 60638:
			return VmoResources.VMO60638;
		case 60650:
			return VmoResources.VMO60650;
		case 60651:
			return VmoResources.VMO60651;
		case 60660:
			return VmoResources.VMO60660;
		case 60661:
			return VmoResources.VMO60661;
		case 60701:
		{
			string text91 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text91) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60701, text91);
		}
		case 60702:
			return VmoResources.VMO60702;
		case 60703:
		{
			string text90 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text90) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60703, text90);
		}
		case 60704:
			return VmoResources.VMO60704;
		case 60705:
			return VmoResources.VMO60705;
		case 61000:
			return VmoResources.VMO61000;
		case 61001:
			return VmoResources.VMO61001;
		case 61002:
			return VmoResources.VMO61002;
		case 61003:
			return VmoResources.VMO61003;
		case 61004:
			return VmoResources.VMO61004;
		case 61005:
			return VmoResources.VMO61005;
		case 61006:
		{
			string text89 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text89) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61006, text89);
		}
		case 61007:
		{
			string text88 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text88) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61007, text88);
		}
		case 61008:
			return VmoResources.VMO61008;
		case 61009:
		{
			string text87 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text87) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61009, text87);
		}
		case 61010:
		{
			string text86 = databaseError.ExtractString("domain");
			parameters = new(int, string, string)[1] { (0, "domain", text86) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61010, text86);
		}
		case 61011:
			return VmoResources.VMO61011;
		case 61012:
			return VmoResources.VMO61012;
		case 61013:
			return VmoResources.VMO61013;
		case 61015:
			return VmoResources.VMO61015;
		case 61021:
			return VmoResources.VMO61021;
		case 61022:
		{
			string text84 = databaseError.ExtractString("command");
			string text85 = databaseError.ExtractString("status");
			parameters = new(int, string, string)[2]
			{
				(0, "command", text84),
				(1, "status", text85)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61022, text84, text85);
		}
		case 61023:
			return VmoResources.VMO61023;
		case 61024:
			return VmoResources.VMO61024;
		case 61025:
			return VmoResources.VMO61025;
		case 61026:
			return VmoResources.VMO61026;
		case 61027:
			return VmoResources.VMO61027;
		case 61028:
			return VmoResources.VMO61028;
		case 61029:
			return VmoResources.VMO61029;
		case 61030:
			return VmoResources.VMO61030;
		case 61031:
			return VmoResources.VMO61031;
		case 61032:
			return VmoResources.VMO61032;
		case 61033:
			return VmoResources.VMO61033;
		case 61034:
			return VmoResources.VMO61034;
		case 61035:
			return VmoResources.VMO61035;
		case 61100:
			return VmoResources.VMO61100;
		case 61103:
			return VmoResources.VMO61103;
		case 61104:
			return VmoResources.VMO61104;
		case 61105:
			return VmoResources.VMO61105;
		case 61106:
			return VmoResources.VMO61106;
		case 61107:
			return VmoResources.VMO61107;
		case 61108:
		{
			string text83 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text83) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO61108, text83);
		}
		case 61109:
			return VmoResources.VMO61109;
		case 61110:
			return VmoResources.VMO61110;
		case 62001:
			return VmoResources.VMO62001;
		case 62002:
			return VmoResources.VMO62002;
		case 62003:
			return VmoResources.VMO62003;
		case 62004:
			return VmoResources.VMO62004;
		case 62005:
			return VmoResources.VMO62005;
		case 62006:
			return VmoResources.VMO62006;
		case 62007:
			return VmoResources.VMO62007;
		case 62008:
			return VmoResources.VMO62008;
		case 62010:
		{
			string text81 = databaseError.ExtractString("argumentName");
			string text82 = databaseError.ExtractString("sid");
			parameters = new(int, string, string)[2]
			{
				(0, "argumentName", text81),
				(1, "sid", text82)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62010, text81, text82);
		}
		case 62011:
		{
			string text79 = databaseError.ExtractString("argumentName");
			string text80 = databaseError.ExtractString("action");
			parameters = new(int, string, string)[2]
			{
				(0, "argumentName", text79),
				(1, "action", text80)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62011, text79, text80);
		}
		case 62012:
		{
			string text77 = databaseError.ExtractString("argumentName");
			string text78 = databaseError.ExtractString("classId");
			parameters = new(int, string, string)[2]
			{
				(0, "argumentName", text77),
				(1, "classId", text78)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62012, text77, text78);
		}
		case 62013:
		{
			string text75 = databaseError.ExtractString("argumentName");
			string text76 = databaseError.ExtractString("sid");
			parameters = new(int, string, string)[2]
			{
				(0, "argumentName", text75),
				(1, "sid", text76)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62013, text75, text76);
		}
		case 62014:
		{
			string text74 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text74) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62014, text74);
		}
		case 62015:
		{
			string text73 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text73) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62015, text73);
		}
		case 62016:
		{
			string text72 = databaseError.ExtractString("sid");
			parameters = new(int, string, string)[1] { (0, "sid", text72) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62016, text72);
		}
		case 62017:
			return VmoResources.VMO62017;
		case 62018:
		{
			string text71 = databaseError.ExtractString("domain");
			parameters = new(int, string, string)[1] { (0, "domain", text71) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62018, text71);
		}
		case 62019:
		{
			string text70 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text70) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62019, text70);
		}
		case 62020:
		{
			string text69 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text69) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62020, text69);
		}
		case 62021:
		{
			string text68 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text68) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62021, text68);
		}
		case 62022:
		{
			string text67 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text67) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62022, text67);
		}
		case 62023:
		{
			string text65 = databaseError.ExtractString("argumentName");
			string text66 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[2]
			{
				(0, "argumentName", text65),
				(1, "name", text66)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62023, text65, text66);
		}
		case 62024:
		{
			string text64 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text64) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62024, text64);
		}
		case 62025:
		{
			string text63 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text63) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62025, text63);
		}
		case 62026:
		{
			string text62 = databaseError.ExtractString("argumentName");
			parameters = new(int, string, string)[1] { (0, "argumentName", text62) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO62026, text62);
		}
		case 62027:
			return VmoResources.VMO62027;
		case 62500:
			return VmoResources.VMO62500;
		case 63007:
		{
			string text61 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text61) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63007, text61);
		}
		case 63008:
		{
			string text59 = databaseError.ExtractString("file");
			string text60 = databaseError.ExtractString("error");
			parameters = new(int, string, string)[2]
			{
				(0, "file", text59),
				(1, "error", text60)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63008, text59, text60);
		}
		case 63009:
			return VmoResources.VMO63009;
		case 63010:
			return VmoResources.VMO63010;
		case 63011:
			return VmoResources.VMO63011;
		case 63012:
			return VmoResources.VMO63012;
		case 63013:
			return VmoResources.VMO63013;
		case 63014:
			return VmoResources.VMO63014;
		case 63015:
		{
			string text58 = databaseError.ExtractString("eventTypeName");
			parameters = new(int, string, string)[1] { (0, "eventTypeName", text58) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63015, text58);
		}
		case 63016:
		{
			string text57 = databaseError.ExtractString("FeatureName");
			parameters = new(int, string, string)[1] { (0, "FeatureName", text57) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO63016, text57);
		}
		case 64000:
			return VmoResources.VMO64000;
		case 64001:
			return VmoResources.VMO64001;
		case 64002:
			return VmoResources.VMO64002;
		case 64003:
			return VmoResources.VMO64003;
		case 64004:
			return VmoResources.VMO64004;
		case 64005:
			return VmoResources.VMO64005;
		case 64006:
			return VmoResources.VMO64006;
		case 64007:
			return VmoResources.VMO64007;
		case 64008:
			return VmoResources.VMO64008;
		case 64009:
			return VmoResources.VMO64009;
		case 64010:
			return VmoResources.VMO64010;
		case 64011:
			return VmoResources.VMO64011;
		case 64012:
			return VmoResources.VMO64012;
		case 64013:
			return VmoResources.VMO64013;
		case 64014:
			return VmoResources.VMO64014;
		case 64015:
			return VmoResources.VMO64015;
		case 64016:
			return VmoResources.VMO64016;
		case 64017:
			return VmoResources.VMO64017;
		case 64018:
			return VmoResources.VMO64018;
		case 64019:
			return VmoResources.VMO64019;
		case 64020:
			return VmoResources.VMO64020;
		case 64021:
			return VmoResources.VMO64021;
		case 64022:
			return VmoResources.VMO64022;
		case 64023:
		{
			string text56 = databaseError.ExtractString("SmartClientOEMName");
			parameters = new(int, string, string)[1] { (0, "SmartClientOEMName", text56) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64023, text56);
		}
		case 64024:
			return VmoResources.VMO64024;
		case 64025:
			return VmoResources.VMO64025;
		case 64026:
			return VmoResources.VMO64026;
		case 64027:
			return VmoResources.VMO64027;
		case 64028:
			return VmoResources.VMO64028;
		case 64029:
			return VmoResources.VMO64029;
		case 64030:
			return VmoResources.VMO64030;
		case 64031:
			return VmoResources.VMO64031;
		case 64032:
			return VmoResources.VMO64032;
		case 64033:
			return VmoResources.VMO64033;
		case 64034:
		{
			string text55 = databaseError.ExtractString("SmartClientOEMName");
			parameters = new(int, string, string)[1] { (0, "SmartClientOEMName", text55) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64034, text55);
		}
		case 64035:
			return VmoResources.VMO64035;
		case 64036:
			return VmoResources.VMO64036;
		case 64037:
			return VmoResources.VMO64037;
		case 64038:
			return VmoResources.VMO64038;
		case 64039:
			return VmoResources.VMO64039;
		case 64040:
			return VmoResources.VMO64040;
		case 64041:
			return VmoResources.VMO64041;
		case 64042:
			return VmoResources.VMO64042;
		case 64043:
			return VmoResources.VMO64043;
		case 64050:
			return VmoResources.VMO64050;
		case 64051:
			return VmoResources.VMO64051;
		case 64052:
			return VmoResources.VMO64052;
		case 64053:
		{
			string text54 = databaseError.ExtractString("OemIcpName");
			parameters = new(int, string, string)[1] { (0, "OemIcpName", text54) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64053, text54);
		}
		case 64054:
			return VmoResources.VMO64054;
		case 64055:
			return VmoResources.VMO64055;
		case 64056:
			return VmoResources.VMO64056;
		case 64057:
			return VmoResources.VMO64057;
		case 64058:
			return VmoResources.VMO64058;
		case 64059:
			return VmoResources.VMO64059;
		case 64060:
			return VmoResources.VMO64060;
		case 64100:
		{
			string text53 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text53) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64100, text53);
		}
		case 64101:
			return VmoResources.VMO64101;
		case 64102:
			return VmoResources.VMO64102;
		case 64103:
		{
			string text52 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text52) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO64103, text52);
		}
		case 64104:
			return VmoResources.VMO64104;
		case 64105:
			return VmoResources.VMO64105;
		case 65000:
		{
			string text49 = databaseError.ExtractString("Status");
			string text50 = databaseError.ExtractString("Namespace");
			string text51 = databaseError.ExtractString("ID");
			parameters = new(int, string, string)[3]
			{
				(0, "Status", text49),
				(1, "Namespace", text50),
				(2, "ID", text51)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65000, text49, text50, text51);
		}
		case 65001:
		{
			string text46 = databaseError.ExtractString("Status");
			string text47 = databaseError.ExtractString("Namespace");
			string text48 = databaseError.ExtractString("ID");
			parameters = new(int, string, string)[3]
			{
				(0, "Status", text46),
				(1, "Namespace", text47),
				(2, "ID", text48)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65001, text46, text47, text48);
		}
		case 65002:
			return VmoResources.VMO65002;
		case 65003:
			return VmoResources.VMO65003;
		case 65004:
			return VmoResources.VMO65004;
		case 65005:
		{
			string text45 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text45) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65005, text45);
		}
		case 65006:
		{
			string text43 = databaseError.ExtractString("Value");
			string text44 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[2]
			{
				(0, "Value", text43),
				(1, "Name", text44)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65006, text43, text44);
		}
		case 65007:
		{
			string text42 = databaseError.ExtractString("Message");
			parameters = new(int, string, string)[1] { (0, "Message", text42) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65007, text42);
		}
		case 65008:
		{
			string text41 = databaseError.ExtractString("Name");
			parameters = new(int, string, string)[1] { (0, "Name", text41) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65008, text41);
		}
		case 65009:
			return VmoResources.VMO65009;
		case 65010:
			return VmoResources.VMO65010;
		case 65011:
		{
			string text40 = databaseError.ExtractString("IDTimeProfile");
			parameters = new(int, string, string)[1] { (0, "IDTimeProfile", text40) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65011, text40);
		}
		case 65012:
		{
			string text39 = databaseError.ExtractString("macaddress");
			parameters = new(int, string, string)[1] { (0, "macaddress", text39) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65012, text39);
		}
		case 65013:
		{
			string text38 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text38) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65013, text38);
		}
		case 65014:
		{
			string text37 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text37) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65014, text37);
		}
		case 65015:
		{
			string text36 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text36) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65015, text36);
		}
		case 65016:
			return VmoResources.VMO65016;
		case 65017:
		{
			string text35 = databaseError.ExtractString("drivernumber");
			parameters = new(int, string, string)[1] { (0, "drivernumber", text35) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65017, text35);
		}
		case 65018:
		{
			string text34 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text34) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65018, text34);
		}
		case 65019:
		{
			string text33 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text33) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65019, text33);
		}
		case 65020:
		{
			string text32 = databaseError.ExtractString("id");
			parameters = new(int, string, string)[1] { (0, "id", text32) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO65020, text32);
		}
		case 65021:
			return VmoResources.VMO65021;
		case 65022:
			return VmoResources.VMO65022;
		case 66000:
		{
			string text31 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text31) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66000, text31);
		}
		case 66001:
		{
			string text30 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text30) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66001, text30);
		}
		case 66002:
		{
			string text29 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text29) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66002, text29);
		}
		case 66003:
		{
			string text28 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text28) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66003, text28);
		}
		case 66004:
		{
			string text27 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text27) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66004, text27);
		}
		case 66005:
		{
			string text26 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text26) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66005, text26);
		}
		case 66006:
		{
			string text25 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text25) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66006, text25);
		}
		case 66007:
		{
			string text24 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text24) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66007, text24);
		}
		case 66008:
		{
			string text23 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text23) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66008, text23);
		}
		case 66009:
		{
			string text22 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text22) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66009, text22);
		}
		case 66010:
		{
			string text21 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text21) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66010, text21);
		}
		case 66011:
		{
			string text20 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text20) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66011, text20);
		}
		case 66012:
		{
			string text19 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text19) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66012, text19);
		}
		case 66013:
		{
			string text18 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text18) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66013, text18);
		}
		case 66014:
		{
			string text17 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text17) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66014, text17);
		}
		case 66015:
		{
			string text16 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text16) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66015, text16);
		}
		case 66016:
		{
			string text15 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text15) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66016, text15);
		}
		case 66017:
		{
			string text14 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text14) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66017, text14);
		}
		case 66018:
		{
			string text13 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text13) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66018, text13);
		}
		case 66019:
		{
			string text12 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text12) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66019, text12);
		}
		case 66020:
		{
			string text11 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text11) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66020, text11);
		}
		case 66030:
			return VmoResources.VMO66030;
		case 66031:
			return VmoResources.VMO66031;
		case 66032:
			return VmoResources.VMO66032;
		case 66033:
			return VmoResources.VMO66033;
		case 66034:
			return VmoResources.VMO66034;
		case 66035:
		{
			string text9 = databaseError.ExtractString("minHardwarePasswordLength");
			string text10 = databaseError.ExtractString("maxHardwarePasswordLength");
			parameters = new(int, string, string)[2]
			{
				(0, "minHardwarePasswordLength", text9),
				(1, "maxHardwarePasswordLength", text10)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO66035, text9, text10);
		}
		case 66040:
			return VmoResources.VMO66040;
		case 66050:
			return VmoResources.VMO66050;
		case 66051:
			return VmoResources.VMO66051;
		case 66052:
			return VmoResources.VMO66052;
		case 66101:
			return VmoResources.VMO66101;
		case 66102:
			return VmoResources.VMO66102;
		case 66103:
			return VmoResources.VMO66103;
		case 66104:
			return VmoResources.VMO66104;
		case 66105:
			return VmoResources.VMO66105;
		case 66106:
			return VmoResources.VMO66106;
		case 66107:
			return VmoResources.VMO66107;
		case 66108:
			return VmoResources.VMO66108;
		case 66109:
			return VmoResources.VMO66109;
		case 66110:
			return VmoResources.VMO66110;
		case 66111:
			return VmoResources.VMO66111;
		case 66112:
			return VmoResources.VMO66112;
		case 66113:
			return VmoResources.VMO66113;
		case 66114:
			return VmoResources.VMO66114;
		case 66115:
			return VmoResources.VMO66115;
		case 67001:
			return VmoResources.VMO67001;
		case 67002:
		{
			string text8 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text8) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67002, text8);
		}
		case 67003:
		{
			string text7 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[1] { (0, "name", text7) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67003, text7);
		}
		case 67004:
		{
			string text4 = databaseError.ExtractString("address");
			string text5 = databaseError.ExtractString("name");
			string text6 = databaseError.ExtractString("exceptionMessage");
			parameters = new(int, string, string)[3]
			{
				(0, "address", text4),
				(1, "name", text5),
				(2, "exceptionMessage", text6)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67004, text4, text5, text6);
		}
		case 67005:
		{
			string text2 = databaseError.ExtractString("address");
			string text3 = databaseError.ExtractString("name");
			parameters = new(int, string, string)[2]
			{
				(0, "address", text2),
				(1, "name", text3)
			};
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67005, text2, text3);
		}
		case 67006:
			return VmoResources.VMO67006;
		case 67007:
			return VmoResources.VMO67007;
		case 67008:
			return VmoResources.VMO67008;
		case 67009:
			return VmoResources.VMO67009;
		case 67010:
		{
			string text = databaseError.ExtractString("message");
			parameters = new(int, string, string)[1] { (0, "message", text) };
			return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO67010, text);
		}
		case 67011:
			return VmoResources.VMO67011;
		case 67012:
			return VmoResources.VMO67012;
		case 67013:
			return VmoResources.VMO67013;
		case 67014:
			return VmoResources.VMO67014;
		case 67015:
			return VmoResources.VMO67015;
		case 67016:
			return VmoResources.VMO67016;
		case 67017:
			return VmoResources.VMO67017;
		case 68000:
			return VmoResources.VMO68000;
		case 69000:
			return VmoResources.VMO69000;
		default:
		{
			string message = string.Format(CultureInfo.InvariantCulture, "SQL error code not supported. Error: {0}, Message: {1}", databaseError.Number, databaseError.Message);
			throw new InvalidOperationException(message);
		}
		}
	}

	public static string ExtractString(this DatabaseError databaseError, string key)
	{
		string text = "%" + key + "=";
		int num = databaseError.Message.IndexOf(text, StringComparison.CurrentCultureIgnoreCase);
		if (num == -1)
		{
			return string.Empty;
		}
		int num2 = databaseError.Message.IndexOf("%", num + 1, StringComparison.CurrentCultureIgnoreCase);
		if (num2 == -1)
		{
			return string.Empty;
		}
		return databaseError.Message.Substring(num + text.Length, num2 - (num + text.Length));
	}
}
