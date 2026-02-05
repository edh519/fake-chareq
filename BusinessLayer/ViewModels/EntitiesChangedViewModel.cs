namespace BusinessLayer.ViewModels
{
    public class EntitiesChangedViewModel
    {
        public EntitiesChangedViewModel(int? changeCount)
        {
            EntitiesChangedCount = changeCount ?? 0;
        }
        public int EntitiesChangedCount { get; set; }
    }
}
