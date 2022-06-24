namespace WARD.Generics;

// Types that a template item can possibly be.
public enum TemplateItemTypeEnum {
    Value, // Parameter is a value replacement.
    Type, // Parameter is a type replacement.
    Concept // Parameter is a concept replacement (can define other templates with this concept).
}