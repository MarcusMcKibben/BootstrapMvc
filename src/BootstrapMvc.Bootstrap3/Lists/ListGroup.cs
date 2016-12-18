﻿namespace BootstrapMvc.Lists
{
    using BootstrapMvc.Core;

    public class ListGroup : ContentElement<ListGroupContent>
    {
        private string endTag;

        protected override ListGroupContent CreateContentContext(IBootstrapContext context)
        {
            return new ListGroupContent(context, this);
        }

        protected override void WriteSelfStart(System.IO.TextWriter writer)
        {
            var tb = Helper.CreateTagBuilder("ul");

            tb.AddCssClass("list-group");
            ApplyCss(tb);
            ApplyAttributes(tb);

            tb.WriteStartTag(writer);

            endTag = tb.GetEndTag();
        }

        protected override void WriteSelfEnd(System.IO.TextWriter writer)
        {
            writer.Write(endTag);
        }
    }
}
