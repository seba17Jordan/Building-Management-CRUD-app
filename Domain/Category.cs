using CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Category
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Category() { }

        public void SelfValidate()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new EmptyFieldException("Category name can't be null or empty");
            }
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            Category category = (Category)obj;
            return Id == category.Id &&
                Name == category.Name;
        }
    }
}
