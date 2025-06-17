namespace Phone_mvc.Models
{
    // <summary>
    /// Base class cho các view model của Phone
    /// </summary>
    public abstract class PhoneViewModelBase
    {
        /// <summary>
        /// Lấy lỗi từ ModelState cho một property cụ thể
        /// </summary>
        public string? GetModelStateError(string propertyName)
        {
            if (ModelState == null || !ModelState.ContainsKey(propertyName))
                return null;

            var errors = ModelState[propertyName]?.Errors;
            if (errors == null || errors.Count == 0)
                return null;

            return errors[0].ErrorMessage;
        }

        /// <summary>
        /// ModelState từ controller
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary? ModelState { get; set; }
    }
}
