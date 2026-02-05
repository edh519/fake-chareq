namespace BusinessLayer.ViewModels
{
    public class EntityChangedViewModel
    {
        public EntityChangedViewModel(int? entityId)
        {
            EntityId = entityId;
        }
        public int? EntityId { get; set; }
    }
}
