using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;

namespace Idera.SQLsecure.UI.Console.Forms
{
    public partial class Form_SnapshotDbObjProperties : Idera.SQLsecure.UI.Console.Controls.BaseDialogForm
    {
        #region Fields

        Sql.ServerVersion m_Version;
        Sql.ObjectTag m_ObjectTag;

        #endregion

        #region Helpers

        private void getNames(
                ref string name,
                ref string owner,
                ref string schema,
                ref string schemaowner
            )
        {
            Sql.DatabaseObject.Properties p = Sql.DatabaseObject.Properties.Get(m_ObjectTag.SnapshotId,
                            m_ObjectTag.DatabaseId, m_ObjectTag.ClassId, m_ObjectTag.ParentObjectId,
                                m_ObjectTag.ObjectId);
            if (p != null)
            {
                name = m_ObjectTag.DatabaseName
                                 + "." + (m_Version == Sql.ServerVersion.SQL2000 ? p.Owner : p.SchemaName)
                                 + "." + m_ObjectTag.ObjectName;
                owner = p.Owner;
                schema = p.SchemaName;
                schemaowner = p.SchemaOwner;
            }
        }

        #endregion

        #region Ctors

        public Form_SnapshotDbObjProperties(
                Sql.ServerVersion version,
                Sql.ObjectTag tag
            )
        {
            InitializeComponent();

            // Update fields.
            m_Version = version;
            m_ObjectTag = tag;

            // Retrieve properties.
            string name = string.Empty, 
                   owner = string.Empty, 
                   schema = string.Empty, 
                   schemaowner = string.Empty;
            getNames(ref name, ref owner, ref schema, ref schemaowner);

            // Set the properties.
            _lbl_Name.Text = name;
            _lbl_Owner.Text = owner;
            if (m_Version == Sql.ServerVersion.SQL2000)
            {
                _lbl_S.Enabled = _lbl_Schema.Enabled = false;
                _lbl_SO.Enabled = _lbl_SchemaOwner.Enabled = false;
                _lbl_Schema.Text = "";
                _lbl_SchemaOwner.Text = "";
            }
            else
            {
                _lbl_Schema.Text = schema;
                _lbl_SchemaOwner.Text = schemaowner;
            }

            // Set title based on type.
            this.Text = tag.TypeName + " Properties - " + name;

            Description = "View properties for this SQL Server " + tag.TypeName;

            switch(tag.ObjType)
            {
                case Sql.ObjectType.TypeEnum.Table:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Table_48;
                    break;
                case Sql.ObjectType.TypeEnum.View:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.View1_48;
                    break;
                case Sql.ObjectType.TypeEnum.Synonym:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Synonym_48;
                    break;
                case Sql.ObjectType.TypeEnum.StoredProcedure:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.StoredProcedure_48;
                    break;
                case Sql.ObjectType.TypeEnum.Function:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.Function_48;
                    break;
                case Sql.ObjectType.TypeEnum.ExtendedStoredProcedure:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.StoredProcedure_48;
                    break;
                case Sql.ObjectType.TypeEnum.Assembly:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.assembly_48;
                    break;
                case Sql.ObjectType.TypeEnum.UserDefinedDataType:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.UserDefinedData_48;
                    break;
                case Sql.ObjectType.TypeEnum.XMLSchemaCollection:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.XMLSchemaCollection_48;
                    break;
                case Sql.ObjectType.TypeEnum.FullTextCatalog:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.FulltextCatalog_48;
                    break;                
                case Sql.ObjectType.TypeEnum.SequenceObjects:
                    Picture = global::Idera.SQLsecure.UI.Console.Properties.Resources.sequence_48;
                    break;   
            }

            // Fill explicit permissions.
            _permissionsGrid.Initialize(m_Version, m_ObjectTag);
        }

        #endregion

        #region Methods

        public static void Process(
                Sql.ServerVersion version,
                Sql.ObjectTag tag)
        {
            Debug.Assert(tag != null);

            Form_SnapshotDbObjProperties form = new Form_SnapshotDbObjProperties(version,tag);
            form.ShowDialog();
        }

        #endregion

    }
}