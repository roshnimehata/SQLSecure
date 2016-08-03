using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Idera.SQLsecure.Core.Logger;
using Idera.SQLsecure.UI.Console.Sql;

namespace Idera.SQLsecure.UI.Console.SQL
{
    public static class TagWorker
    {
        private const string GetTagsSp = "SQLsecure.dbo.isp_GetTags";
        private const string GetServersByTagSp = "SQLsecure.dbo.isp_GetServersByTag";
        private const string UpdateInsertTagSp = "SQLsecure.dbo.isp_InsertUpdateTag";
        private const string AssigneTagToServersSp = "SQLsecure.dbo.isp_AssignTagToServers";
        private const string AssigneTagsToServerSp = "SQLsecure.dbo.isp_AssignTagsToServer";
        private const string RemoveServerFromTagSp = "SQLsecure.dbo.isp_RemoveServerFromTag";
        private const string DeleteTagSp = "SQLsecure.dbo.isp_DeleteTag";


        private static BBS.TracerX.Logger logX = new LogX("Idera.SQLsecure.UI.Console.Sql.Tag").loggerX;

        public static List<Tag> GetTags()
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        using (SqlDataReader rdr = Sql.SqlHelper.ExecuteReader(conn, null, CommandType.StoredProcedure, GetTagsSp, null))
                        {
                            return GetTagFromReader(rdr);
                        }
                    }

                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }



        public static Tag GetTagById(int id)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        using (SqlDataReader rdr = SqlHelper.ExecuteReader(conn, null, CommandType.StoredProcedure, GetTagsSp, new[]
                        {
                            new SqlParameter("@tag_id",id)
                        }))
                        {
                            var res = GetTagFromReader(rdr);
                            if (res.Count != 0)
                            {
                                return res[0];
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }

        public static void AssignServerToTag(int id, List<string> servers)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, AssigneTagToServersSp,
                            new[]
                        {
                            new SqlParameter("@tag_id",id),
                            new SqlParameter("@servers",string.Join(",",servers.ToArray()))
                        });

                    }
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }
        public static Tag AssignServerToTags(int serverId, List<string> tags)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, AssigneTagsToServerSp,
                            new[]
                        {
                            new SqlParameter("@tag_ids",string.Join(",",tags.ToArray())),
                            new SqlParameter("@server_id",serverId)
                        });

                    }
                    return null;
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }
        public static Tag GetTagByName(string name)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        using (SqlDataReader rdr = SqlHelper.ExecuteReader(conn, null, CommandType.StoredProcedure, GetTagsSp, new[]
                        {
                            new SqlParameter("@tag_name",name)
                        }))
                        {
                            var res = GetTagFromReader(rdr);
                            if (res.Count != 0)
                            {
                                return res[0];
                            }
                        }
                    }
                    return null;
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }
        public static List<TaggedServer> GetTagServers(int tagId)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        using (SqlDataReader rdr = SqlHelper.ExecuteReader(conn, null, CommandType.StoredProcedure, GetServersByTagSp, new[]
                        {
                            new SqlParameter("@tag_id",tagId)
                        }))
                        {
                            List<TaggedServer> servers = new List<TaggedServer>();
                            while (rdr.Read())
                            {
                                servers.Add(MapServerTag(rdr));
                            }
                            return servers;
                        }
                    }

                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }

        public static void UpdateCreateTag(Tag tag)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, UpdateInsertTagSp, TagToSqlParameters(tag));
                    }
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }

        public static void DeleteTag(int tagId)
        {
            using (logX.InfoCall())
            {
                try
                {
                    var tag = GetTagById(tagId);
                    if (tag.IsDefault) throw new ApplicationException("Default tag can't be removed!");
                    if (tag.TaggedServers.Count != 0) throw new ApplicationException("Tags assigned to servers can't be removed! Please remove all associated Servers before proceeding.");
                   

                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, DeleteTagSp,
                            new SqlParameter("@tag_id", tagId));
                    }
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }

        public static void RemoveServerFromTag(int tagId, int serverId)
        {
            using (logX.InfoCall())
            {
                try
                {
                    using (var conn = Program.gController.Repository.GetOpennedConnection())
                    {
                        SqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, RemoveServerFromTagSp, new[] {
                             new SqlParameter("@tag_id",tagId),
                              new SqlParameter("@server_id",serverId)
                        }
                            );
                    }
                }
                catch (Exception ex)
                {
                    logX.Error("Error during getting list of tags", ex);
                    throw;
                }
            }
        }
        private static List<Tag> GetTagFromReader(SqlDataReader rdr)
        {
            using (logX.InfoCall())
            {
                List<Tag> tags = new List<Tag>();

                while (rdr.Read())
                {
                    tags.Add(MapTag(rdr));
                }
                if (rdr.NextResult())
                {
                    List<TaggedServer> servers = new List<TaggedServer>();
                    while (rdr.Read())
                    {
                        servers.Add(MapServerTag(rdr));
                    }
                    foreach (Tag tag in tags)
                    {
                        var tagServers = servers.FindAll(a => a.TagId == tag.Id);
                        if (tagServers.Count != 0)
                        {
                            tag.TaggedServers.AddRange(tagServers);
                        }
                    }
                }
                return tags;
            }
        }

        private static SqlParameter[] TagToSqlParameters(Tag updateTag)
        {
            return new[]
            {
                new SqlParameter("@tag_name",updateTag.Name),
                new SqlParameter("@description",updateTag.Description),
                new SqlParameter("@tag_id",updateTag.Id<=0?(int?)null:updateTag.Id)

            };
        }

        private static Tag MapTag(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                var resTag = new Tag();
                resTag.Id = int.Parse(reader["tag_id"].ToString());
                resTag.Name = reader["name"].ToString();
                resTag.Description = reader["description"].ToString();
                resTag.IsDefault = bool.Parse(reader["is_default"].ToString());
                return resTag;
            }
            return null;
        }

        private static TaggedServer MapServerTag(SqlDataReader reader)
        {
            if (reader.HasRows)
            {
                var resTag = new TaggedServer();
                resTag.TagId = int.Parse(reader["tag_id"].ToString());
                resTag.Name = reader["servername"].ToString();
                resTag.Id = int.Parse(reader["registeredserverid"].ToString());

                return resTag;
            }
            return null;
        }


    }
}
