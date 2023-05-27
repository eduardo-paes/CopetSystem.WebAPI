using System;
using Domain.Entities.Primitives;
using Domain.Validation;

namespace Domain.Entities
{
    /// <summary>
    /// Área de conhecimento
    /// </summary>
    public class Area : Entity
    {
        #region Properties
        private Guid? _mainAreaId;
        public Guid? MainAreaId
        {
            get { return _mainAreaId; }
            set
            {
                EntityExceptionValidation.When(!value.HasValue,
                    ExceptionMessageFactory.Invalid("mainAreaId"));
                _mainAreaId = value;
            }
        }
        private string? _code;
        public string? Code
        {
            get { return _code; }
            set
            {
                EntityExceptionValidation.When(string.IsNullOrEmpty(value),
                    ExceptionMessageFactory.Required("code"));
                EntityExceptionValidation.When(value?.Length < 3,
                    ExceptionMessageFactory.MinLength("code", 3));
                EntityExceptionValidation.When(value?.Length > 100,
                    ExceptionMessageFactory.MaxLength("code", 100));
                _code = value;
            }
        }
        private string? _name;
        public string? Name
        {
            get { return _name; }
            set
            {
                EntityExceptionValidation.When(string.IsNullOrEmpty(value),
                ExceptionMessageFactory.Required("name"));
                EntityExceptionValidation.When(value?.Length < 3,
                    ExceptionMessageFactory.MinLength("name", 3));
                EntityExceptionValidation.When(value?.Length > 300,
                    ExceptionMessageFactory.MaxLength("name", 300));
                _name = value;
            }
        }
        public virtual MainArea? MainArea { get; }
        #endregion

        #region Constructors
        public Area(Guid? mainAreaId, string? code, string? name)
        {
            MainAreaId = mainAreaId;
            Code = code;
            Name = name;
        }

        public Area(Guid id, Guid? mainAreaId, string? code, string? name, MainArea? mainArea = null)
        {
            Id = id;
            MainAreaId = mainAreaId;
            Code = code;
            Name = name;
            MainArea = mainArea;
        }

        /// <summary>
        /// Constructor to dbcontext EF instancing.
        /// </summary>
        protected Area() { }
        #endregion
    }
}