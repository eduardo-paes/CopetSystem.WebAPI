using System.Data;
using System.Linq;
using System.Net.Mail;
using Domain.Entities.Enums;
using Domain.Entities.Primitives;
using Domain.Validation;

namespace Domain.Entities
{
    public class User : Entity
    {
        #region Properties
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                DomainExceptionValidation.When(string.IsNullOrEmpty(value),
                    ExceptionMessageFactory.Required("name"));
                DomainExceptionValidation.When(value?.Length < 3,
                    ExceptionMessageFactory.MinLength("name", 3));
                DomainExceptionValidation.When(value?.Length > 300,
                    ExceptionMessageFactory.MaxLength("name", 300));
                _name = value;
            }
        }

        private string? _email;
        public string? Email
        {
            get { return _email; }
            set
            {
                DomainExceptionValidation.When(string.IsNullOrEmpty(value),
                    ExceptionMessageFactory.Required("email"));
                DomainExceptionValidation.When(value?.Length > 300,
                    ExceptionMessageFactory.MaxLength("email", 300));
                DomainExceptionValidation.When(ValidateEmail(value!),
                    ExceptionMessageFactory.InvalidEmail("email"));
                _email = value;
            }
        }

        private string? _password;
        public string? Password
        {
            get { return _password; }
            set
            {
                DomainExceptionValidation.When(string.IsNullOrEmpty(value),
                    ExceptionMessageFactory.Required("password"));
                DomainExceptionValidation.When(value?.Length < 6,
                    ExceptionMessageFactory.MinLength("password", 6));
                DomainExceptionValidation.When(value?.Length > 300,
                    ExceptionMessageFactory.MaxLength("password", 300));
                _password = value;
            }
        }

        private string? _cpf;
        public string? CPF
        {
            get { return _cpf; }
            set
            {
                DomainExceptionValidation.When(string.IsNullOrEmpty(value),
                    ExceptionMessageFactory.Required("cpf"));

                // Extract only numbers from cpf
                value = GetOnlyNumbers(value);
                DomainExceptionValidation.When(value?.Length != 11,
                    ExceptionMessageFactory.WithLength("cpf", 11));
                DomainExceptionValidation.When(ValidateCPF(value!),
                    ExceptionMessageFactory.InvalidCpf());
                _cpf = value;
            }
        }

        private ERole? _role;
        public ERole? Role
        {
            get { return _role; }
            set
            {
                DomainExceptionValidation.When(value == null,
                    ExceptionMessageFactory.Required("role"));
                _role = value;
            }
        }
        public bool IsConfirmed { get; private set; }

        private string? _validationCode;
        public string? ValidationCode
        {
            get { return _validationCode; }
            private set
            {
                if (!IsConfirmed)
                {
                    DomainExceptionValidation.When(string.IsNullOrEmpty(value),
                        ExceptionMessageFactory.Required("validationCode"));
                    DomainExceptionValidation.When(value?.Length > 6,
                        ExceptionMessageFactory.MaxLength("validationCode", 6));
                    _validationCode = value;
                }
            }
        }

        #endregion

        #region Constructors
        public User(string? name, string? email, string? password, string? cpf, ERole? role, DateTime? deletedAt)
        {
            Name = name;
            Email = email;
            Password = password;
            CPF = cpf;
            Role = role;
            DeletedAt = deletedAt;
        }

        internal User(string? name, string? email, string? password, string? cpf, ERole? role)
        {
            Name = name;
            Email = email;
            Password = password;
            CPF = cpf;
            Role = role;

            GenerateValidationCode();
        }

        /// <summary>
        /// Constructor to dbcontext EF instancing.
        /// </summary>
        protected User() { }
        #endregion

        #region Utils
        internal void GenerateValidationCode()
        {
            IsConfirmed = false;
            var random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            ValidationCode = new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        internal void ConfirmUser(string validationCode)
        {
            DomainExceptionValidation.When(IsConfirmed,
                "O e-mail do usuário já foi confirmado.");
            DomainExceptionValidation.When(ValidationCode != validationCode,
                "Código de validação inválido.");
            IsConfirmed = true;
            ValidationCode = null;
        }

        private static bool ValidateEmail(string email)
        {
            try
            {
                return new MailAddress(email) == null;
            }
            catch
            {
                return true;
            }
        }

        private static bool ValidateCPF(string cpf)
        {
            cpf = cpf.Trim().Replace(".", "").Replace("-", "");

            // Check length
            if (cpf.Length != 11)
                return false;

            // Sum first digit
            int sum = 0;
            for (int i = 1; i < 10; i++)
            {
                sum += int.Parse(cpf[i - 1].ToString()) * (11 - i);
            }

            // Get first digit
            int dig1 = 11 - (sum % 11);
            if (dig1 > 9) dig1 = 0;

            // Sum second digit
            sum = 0;
            for (int i = 1; i < 11; i++)
            {
                sum += int.Parse(cpf[i - 1].ToString()) * (12 - i);
            }

            // Get second digit
            int dig2 = 11 - (sum % 11);
            if (dig2 > 9) dig2 = 0;

            // Check if CPF ends with correct digits
            string digit = dig1.ToString() + dig2.ToString();
            return !cpf.EndsWith(digit);
        }

        private static string? GetOnlyNumbers(string? input) => !string.IsNullOrEmpty(input)
            ? string.Concat(input.Where(char.IsDigit))
            : input;
        #endregion
    }
}