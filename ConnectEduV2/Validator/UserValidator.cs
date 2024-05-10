using ConnectEduV2.Models;
using FluentValidation;

namespace ConnectEduV2.Validator
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            RuleFor(user => user.Name)
                .NotNull().WithMessage("Tên không được để trống")
                .MaximumLength(100).WithMessage("Tên không được vượt quá 100 ký tự")
                ;

            RuleFor(user => user.Email)
                .NotNull().WithMessage("Email không được để trống")
                .EmailAddress().WithMessage("Định dạng Email không hợp lệ")
                .MaximumLength(255).WithMessage("Email không được vượt quá 255 ký tự");

            RuleFor(user => user.Password)
                .NotEmpty().WithMessage("Mật khẩu không được để trống")
                .MinimumLength(6).WithMessage("Mật khẩu phải có ít nhất 6 ký tự");

            RuleFor(user => user.Image)
                .MaximumLength(255).WithMessage("Đường dẫn hình ảnh không được vượt quá 255 ký tự");



            RuleFor(user => user.ScoreboardPhoto)
                .MaximumLength(255).WithMessage("Đường dẫn hình ảnh Scoreboard không được vượt quá 255 ký tự");


        }
    }
}
