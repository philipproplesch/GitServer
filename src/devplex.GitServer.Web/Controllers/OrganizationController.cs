using System;
using System.Collections.Generic;
using System.Web.Http;

namespace devplex.GitServer.Web.Controllers
{
    public class Organization
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
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
        public string Slug { get; set; }
        public string[] Branches { get; set; }

        public Project()
        {
            Id = Guid.NewGuid();
            Branches = new [] {"master", "next", "feature/foo", "feature/bar"};
        }
    }

    public class OrganizationController : ApiController
    {
        public IEnumerable<Organization> GetOrganizations()
        {
            var response = new List<Organization>();

            var projects = new List<Project> {
                new Project {
                    Name = "First",
                    Slug = "first"
                },
                new Project {
                    Name = "Second",
                    Slug = "second"
                },
                new Project {
                    Name = "Third",
                    Slug = "third"
                }
            };

            response.Add(new Organization {
                Name = "Acme Inc.",
                Slug = "acme-inc",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "devplex",
                Slug = "devplex",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "Foo Corp.",
                Slug = "foo-corp",
                Projects = projects
            });

            response.Add(new Organization {
                Name = "Microsoft",
                Slug = "msft"
            });

            response.Add(new Organization {
                Name = "Google",
                Slug = "google"
            });

            return response;
        }
    }
}