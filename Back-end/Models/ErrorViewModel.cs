namespace Backend.Models
{
    // Is used primarily for displaying error information in the application's views.
    // When an error occurs, the controller can create an instance of this class with a relevant error message and pass it to a view.
    // The view then uses this model to display the error message to the user
    public class ErrorViewModel
    {
        public string? Message { get; set; }

        public ErrorViewModel() { }

        public ErrorViewModel(string? message)
        {
            Message = message;
        }
    }
}