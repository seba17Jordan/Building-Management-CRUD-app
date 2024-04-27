using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelsApi.In
{
    public class CategoryRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Category ToEntity()
        {
            return new Category()
            {
                Id = Id,
                Name = Name
            };
        }
    }
}
