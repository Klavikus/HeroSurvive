using CodeBase.Domain.Data;

namespace CodeBase.ForSort
{
    public class InputController : InputHandler
    {
        private readonly string Horizontal = "Horizontal";
        private readonly string Vertical = "Vertical";

        private void Update()
        {
            float horizontal = SimpleInput.GetAxis(Horizontal);
            float vertical = SimpleInput.GetAxis(Vertical);

            InvokeInputUpdated(new InputData(horizontal, vertical));
        }
    }
}