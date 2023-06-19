import { ComparisonOperator } from "src/app/core/enums/comparison-operator.enum";

export const comparisonOperators = [
    {
        name:"Equal",
        value:ComparisonOperator.Equal,
        icon:"las la-equals"
    },
    {
        name:"Not Equal",
        value:ComparisonOperator.NotEqual,
        icon:"las la-not-equal"
    },
    {
        name:"Greater than",
        value:ComparisonOperator.GreaterThan,
        icon:"las la-greater-than"
    },
    {
        name:"Greater than Or Equal to",
        value:ComparisonOperator.GreaterThanOrEqual,
        icon:"las la-greater-than-equal"
    },
    {
        name:"Less than",
        value:ComparisonOperator.LessThan,
        icon:"las la-less-than"
    },
    {
        name:"Less than Or Equal to",
        value:ComparisonOperator.LessThanOrEqual,
        icon:"las la-less-than-equal"
    },
    {
        name:"Contains",
        value:ComparisonOperator.Contains,
        icon:"las la-text-width"
    },
    {
        name:"Starts With",
        value:ComparisonOperator.StartsWith,
        icon:"las la-align-left"
    },
    {
        name:"Ends With",
        value:ComparisonOperator.EndsWith,
        icon:"las la-align-right"
    },
]