using HackathonApi.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HackathonApi.Data
{
    public static class DbInitializer
    {
        /// <summary>
        /// Prepares the Database and seeds data as required
        /// </summary>
        /// <param name="serviceProvider">DI Container</param>
        /// <param name="DeleteDatabase">Delete the database and start from scratch</param>
        /// <param name="UseMigrations">Use Migrations or EnsureCreated</param>
        /// <param name="SeedSampleData">Add optional sample data</param>
        public static void Initialize(IServiceProvider serviceProvider,
            bool DeleteDatabase = false, bool UseMigrations = true, bool SeedSampleData = true)
        {
            using (var context = new HackathonContext(
                serviceProvider.GetRequiredService<DbContextOptions<HackathonContext>>()))
            {
                #region Prepare the Database
                try
                {
                    if (DeleteDatabase || !context.Database.CanConnect())
                    {
                        context.Database.EnsureDeleted();

                        if (UseMigrations)
                        {
                            context.Database.Migrate();
                        }
                        else
                        {
                            context.Database.EnsureCreated();
                        }

                        string[] triggers =
                        {
                            "SetMemberTimestampOnUpdate",
                            "SetMemberTimestampOnInsert",
                            "SetChallengeTimestampOnUpdate",
                            "SetChallengeTimestampOnInsert",
                            "SetRegionTimestampOnUpdate",
                            "SetRegionTimestampOnInsert"
                        };

                        foreach (var t in triggers)
                        {
                            context.Database.ExecuteSqlRaw($"DROP TRIGGER IF EXISTS {t};");
                        }

                        string sqlCmd = @"
                            CREATE TRIGGER SetMemberTimestampOnUpdate
                            AFTER UPDATE ON Members
                            BEGIN
                                UPDATE Members
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetMemberTimestampOnInsert
                            AFTER INSERT ON Members
                            BEGIN
                                UPDATE Members
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetChallengeTimestampOnUpdate
                            AFTER UPDATE ON Challenges
                            BEGIN
                                UPDATE Challenges
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetChallengeTimestampOnInsert
                            AFTER INSERT ON Challenges
                            BEGIN
                                UPDATE Challenges
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetRegionTimestampOnUpdate
                            AFTER UPDATE ON Regions
                            BEGIN
                                UPDATE Regions
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);

                        sqlCmd = @"
                            CREATE TRIGGER SetRegionTimestampOnInsert
                            AFTER INSERT ON Regions
                            BEGIN
                                UPDATE Regions
                                SET RowVersion = randomblob(8)
                                WHERE rowid = NEW.rowid;
                            END;
                        ";
                        context.Database.ExecuteSqlRaw(sqlCmd);
                    }
                    else
                    {
                        if (UseMigrations)
                        {
                            context.Database.Migrate();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion

                #region Seed Required Data
                try
                {
                    // To randomly generate data
                    Random random = new Random();

                    // Regions
                    if (!context.Regions.Any())
                    {
                        var regions = new List<Region>
                        {
                            new Region { Code = "ON", Name = "Ontario" },
                            new Region { Code = "PE", Name = "Prince Edward Island" },
                            new Region { Code = "NB", Name = "New Brunswick" },
                            new Region { Code = "BC", Name = "British Columbia" },
                            new Region { Code = "NL", Name = "Newfoundland and Labrador" },
                            new Region { Code = "SK", Name = "Saskatchewan" },
                            new Region { Code = "NS", Name = "Nova Scotia" },
                            new Region { Code = "MB", Name = "Manitoba" },
                            new Region { Code = "QC", Name = "Quebec" },
                            new Region { Code = "YT", Name = "Yukon" },
                            new Region { Code = "NU", Name = "Nunavut" },
                            new Region { Code = "NT", Name = "Northwest Territories" },
                            new Region { Code = "AB", Name = "Alberta" }
                        };

                        context.Regions.AddRange(regions);
                        context.SaveChanges();
                    }

                    // Challenges
                    if (!context.Challenges.Any())
                    {
                        string[] challenges =
                        {
                            "Artificial Intelligence", "Web Development", "Cyber Security",
                            "Cloud Computing", "Mobile Applications", "Data Analytics",
                            "DevOps Automation", "Game Development", "Blockchain",
                            "Internet of Things", "Machine Learning", "UI/UX Design",
                            "AR/VR", "Embedded Systems", "FinTech", "HealthTech",
                            "EdTech", "Green Tech", "Smart Cities", "Open Innovation"
                        };

                        string[] challengeCodes =
                        {
                            "AIH","WEB","CYB","CLD","MOB","DAT","DEV","GAM","BLC","IOT",
                            "MLN","UIX","ARV","EMB","FIN","HLT","EDU","GRN","SMC","OPN"
                        };

                        for (int i = 0; i < challenges.Length; i++)
                        {
                            context.Challenges.Add(new Challenge
                            {
                                Code = challengeCodes[i],
                                Name = challenges[i]
                            });
                        }

                        context.SaveChanges();
                    }

                    // Primary Keys
                    int[] challengeIDs = context.Challenges.Select(c => c.ID).ToArray();
                    int[] regionIDs = context.Regions.Select(r => r.ID).ToArray();

                    // Organizations
                    string[] organizations =
                    {
                        "Ontario Tech Collective", "Atlantic Code Guild", "Prairie Dev Network",
                        "Pacific AI Group", "Northern Innovators", "Maple Leaf Software",
                        "Niagara Tech Hub", "Toronto Developers Association",
                        "Canadian Cyber Alliance", "Cloud North", "Startup Ontario",
                        "Women in Tech Canada", "Open Source Canada", "Future Coders",
                        "Hack the North", "Code for Canada", "Data Science Society",
                        "AI Canada", "DevOps Nation", "Blockchain Toronto",
                        "Green Tech Canada", "Digital Health Alliance",
                        "EdTech Canada", "FinTech North"
                    };

                    // Names
                    string[] firstNames =
                    {
                        "Lyric","Antoinette","Kendal","Vivian","Ruth","Jamison","Emilia",
                        "Natalee","Yadiel","Jakayla","Lukas","Moses","Kyler","Karla",
                        "Chanel","Tyler","Camilla","Quintin","Braden","Clarence"
                    };

                    string[] lastNames = { "Watts", "Randall", "Arias", "Weber", "Stone" };

                    // Members
                    if (!context.Members.Any())
                    {
                        int memberCounter = 1;

                        foreach (string lastName in lastNames)
                        {
                            HashSet<string> selectedFirstNames = new HashSet<string>();

                            while (selectedFirstNames.Count < 2)
                            {
                                selectedFirstNames.Add(firstNames[random.Next(firstNames.Length)]);
                            }

                            foreach (string firstName in selectedFirstNames)
                            {
                                // Age 12–30 guaranteed
                                int age = random.Next(12, 31);
                                DateTime dob = DateTime.Today.AddYears(-age);

                                Member member = new Member
                                {
                                    FirstName = firstName,
                                    LastName = lastName,
                                    MiddleName = lastName[1].ToString().ToUpper(),

                                    // Guaranteed unique & valid
                                    MemberCode = (1000000 + memberCounter).ToString(),

                                    DOB = dob,

                                    // Valid ranges
                                    SkillRating = random.Next(1, 11),
                                    YearsExperience = random.Next(0, Math.Min(age, 21)),

                                    Category = (memberCounter % 2 == 0) ? "J" : "A",
                                    Organization = organizations[random.Next(organizations.Length)],
                                    RegionID = regionIDs[random.Next(regionIDs.Length)],
                                    ChallengeID = challengeIDs[random.Next(challengeIDs.Length)]
                                };

                                memberCounter++;

                                context.Members.Add(member);
                            }
                        }

                        context.SaveChanges();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.GetBaseException().Message);
                }
                #endregion
            }
        }
    }
}