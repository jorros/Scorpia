namespace UI.ActionBar
{
    public class ActionButtonOptions
    {
        public int? Progress { get; set; }
        
        public ActionButtonType? Type { get; set; }
        
        public int? UpgradeLevel { get; set; }
        
        public bool Disabled { get; set; }

        public enum ActionButtonType
        {
            InProgress,
            Upgrade
        }
    }
}