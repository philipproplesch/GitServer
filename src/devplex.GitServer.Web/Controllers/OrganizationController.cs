using System;
using System.Collections.Generic;
using System.Web.Http;

namespace devplex.GitServer.Web.Controllers
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Project> Projects { get; set; }

        public Organization()
        {
            Id = Guid.NewGuid();
            Projects = new List<Project>();
        }
    }

    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public Project()
        {
            Id = Guid.NewGuid();
        }
    }

    public class OrganizationController : ApiController
    {
        public IEnumerable<Organization> GetOrganizations()
        {
            var response = new List<Organization>();

            var projects = new List<Project> {
                new Project {
                    Name = "First"
                },
                new Project {
                    Name = "Second"
                },
                new Project {
                    Name = "Third"
                }
            };

            response.Add(new Organization {
                Name = "Acme Inc.",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "devplex",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "Foo Corp.",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "Microsoft"
            });

            response.Add(new Organization {
                Name = "Google"
            });

            return response;
        }
    }
}