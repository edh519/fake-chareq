namespace BusinessLayer.ViewModels
{
    public class PopupMessageViewModel
    {
        public PopupMessageViewModel()
        {

        }

        public PopupMessageViewModel(bool isSuccessful)
        {
            IsSuccessful = isSuccessful;
        }

        public PopupMessageViewModel(bool isSuccessful, string message)
        {
            IsSuccessful = isSuccessful;
            Message = message;
        }


        const string _holdingMessage = "Undetermined error.";
        public bool IsSuccessful { get; set; }
        private string message = _holdingMessage;

        public string Message
        {
            get
            {
                if (IsSuccessful && message == _holdingMessage)
                {
                    return " ";
                }
                else
                {
                    return message;
                }
            }
            set { message = value; }
        }
    }
}
