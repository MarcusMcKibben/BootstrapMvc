﻿namespace BootstrapMvc.Controls
{
    using System;
    using System.Globalization;
    using BootstrapMvc.Core;
    using BootstrapMvc.Forms;

    public class Textarea : Element, IFormControl, IPlaceholderTarget, IGridSizable
    {
        public static readonly byte RowsDefault = 3;

        public GridSize Size { get; set; }

        public int Rows { get; set; } = RowsDefault;

        public bool Disabled { get; set; }

        protected override void WriteSelf(System.IO.TextWriter writer)
        {
            var formContext = GetNearestParent<IForm>();
            var formGroupContext = GetNearestParent<FormGroup>();
            var controlContext = GetNearestParent<IControlContext>();

            ITagBuilder div = null;

            if (!Size.IsEmpty())
            {
                // Inline forms does not support sized controls (we need 'some other' sizing rules?)
                if (formContext != null && formContext.Type != FormType.Inline)
                {
                    if (formGroupContext != null && formGroupContext.WithSizedControl)
                    {
                        div = Helper.CreateTagBuilder("div");
                        div.AddCssClass(Size.ToCssClass());
                        div.WriteStartTag(writer);
                    }
                    else
                    {
                        throw new InvalidOperationException("Size not allowed - call WithSizedControls() on FormGroup.");
                    }
                }
            }

            var tb = Helper.CreateTagBuilder("textarea");
            tb.AddCssClass("form-control");

            var controlSize = ((IControlSizable)formGroupContext)?.Size ?? ControlSize.Default;
            if (controlSize != ControlSize.Default)
            {
                tb.AddCssClass(controlSize.ToFormLabelCssClass());
            }

            if (Rows != 0)
            {
                tb.MergeAttribute("rows", Rows.ToString(CultureInfo.InvariantCulture), true);
            }

            if (controlContext != null)
            {
                tb.MergeAttribute("id", controlContext.FieldName, true);
                tb.MergeAttribute("name", controlContext.FieldName, true);
                if (controlContext.IsRequired)
                {
                    tb.MergeAttribute("required", "required", true);
                }
                
                if (controlContext.HasErrors || controlContext.HasWarning)
                {
                    tb.AddCssClass("is-invalid");
                }
            }

            if (Disabled)
            {
                tb.MergeAttribute("disabled", "disabled", true);
            }

            ApplyCss(tb);
            ApplyAttributes(tb);

            tb.WriteStartTag(writer);

            if (controlContext != null && controlContext.FieldValue != null)
            {
                writer.Write(Helper.HtmlEncode(controlContext.FieldValue.ToString()));
            }

            tb.WriteEndTag(writer);

            if (div != null)
            {
                div.WriteEndTag(writer);
            }
        }
    }
}
