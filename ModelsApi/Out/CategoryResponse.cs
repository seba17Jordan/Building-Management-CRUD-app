using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.Out
{
    public class CategoryResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public CategoryResponse(Domain.Category category)
        {
            Id = category.Id;
            Name = category.Name;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            CategoryResponse other = (CategoryResponse)obj;
            return Id == other.Id && Name == other.Name;
        }   
    }
}
