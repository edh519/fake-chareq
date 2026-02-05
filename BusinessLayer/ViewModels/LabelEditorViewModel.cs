namespace BusinessLayer.ViewModels
{
    public class LabelEditorViewModel
    {
        public int WorkRequestId { get; set; }
        public LabelsViewModel LabelsAssigned { get; set; }
        public LabelsViewModel AllLabels { get; set; }
        public bool IsLoggedInUserOfRoleTypeUser { get; set; }
    }
}
