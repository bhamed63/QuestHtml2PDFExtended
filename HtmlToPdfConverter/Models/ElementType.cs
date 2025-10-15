namespace HtmlToPdfConverter.Models
{
    public enum ElementType
    {
        // text elements
        h1,
        h2,
        h3,
        h4,
        h5,
        h6,
        p,
        span,
        div,
        // layout elements
        table,
        tr,
        td,
        th,
        // list elements
        ul,
        ol,
        li,
        // image elements
        img,
        // others
        br,
        hr,
        a,
        b,
        i,
        u,
        strong,
        em,
        del,
        code,
        blockquote,
        pre,
        sub,
        sup,
        small,
        mark,
        ins,
        // root element
        body,
        // unknown
        unknown
    }
}
