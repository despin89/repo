namespace GD.Core
{
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using Game.ActionsSystem;
    using Game.StatusesSystem;

    public static class TooltipFormatter
    {
        public static string GetStatusTooltip(string tooltip, Status status)
        {
            string pattern = @"\{(.*?)\}";
            string[] fieldsNames = Regex.Matches(tooltip, pattern).Cast<Match>().Select(m => m.Value).ToArray();
            StringBuilder sb = new StringBuilder(tooltip);

            foreach (string fieldName in fieldsNames)
            {
                string value;
                if (fieldName.Contains('.'))
                {
                    string[] query = fieldName.Split('.');
                    ComponentBase componentBase = status.GetComponentByTag(query.First());
                    value = componentBase[query.Last()].ToString();
                }
                else
                {
                    value = status.GetType().GetField(fieldName).GetValue(status).ToString();
                }
                sb.Replace(fieldName, value);
            }

            sb.Replace("{", "").Replace("}", "");

            return sb.ToString();
        }

        //public static string GetTooltipFromAddComponentStatus(string tooltip, string fields, AddComponentStatus o)
        //{
        //    var fieldsList = fields.Split('|');
        //    var values = new List<object>();
        //    foreach (var item in fieldsList)
        //    {
        //        var field = item.Split(':');
        //        var comp = o.ComponentsToAdd.First(e => e.GetType() == Type.GetType(field.First()));
        //        var value = comp.GetType().GetField(field.Last()).GetValue(comp);
        //        values.Add(value);
        //    }
        //    return tooltip.F(values.ToArray());
        //}
    }
}