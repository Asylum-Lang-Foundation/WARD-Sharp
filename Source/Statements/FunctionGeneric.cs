using WARD.Builder;
using WARD.Common;
using WARD.Exceptions;
using WARD.Generics;
using WARD.Types;

namespace WARD.Statements;

// Create a function.
public class FunctionGeneric : Function {
    public Template Template { get; } // Template that the function use

    // Create a new generic function. WARNING: This does not add the function to the scope (do it manually to add both the mangled variable name and regular function name).
    public FunctionGeneric(string name, VarTypeFunction signature, Template template, params ItemAttribute[] attributes) : base(name, signature, attributes) {
        Template = template;
    }

    // If the function can be initialized by the parameters only (no explicit template initialization).
    public bool ImplicitTemplateInitializationPossible() {
        bool[] templateParametersSatisfied = new bool[Template.Items.Length];
        foreach (var p in (Type as VarTypeFunction).Parameters) {
            VarTypeAlias genericAlias = p.Type as VarTypeAlias;
            if (genericAlias != null) {
                for (int i = 0; i < Template.Items.Length; i++) {
                    templateParametersSatisfied[i] |= Template.Items[i].Name.Equals(genericAlias.Alias);
                }
            }
        }
        return templateParametersSatisfied.Where(x => !x).Count() < 1; // Make sure there are no unsatisfied template parameters.
    }

    // If a function call is satisfied by the template. Distance is how many implicit casts need to be done.
    public bool CallSatisfiesTemplate(VarType[] args, out int distance, out Dictionary<string, VarType> allocatedTypes) {

        // Initialize basic veriables.
        distance = 0;
        var parameters = (Type as VarTypeFunction).Parameters;
        allocatedTypes = new Dictionary<string, VarType>();

        // Variadic call check.
        if (args.Length != parameters.Count()) {
            throw new System.NotImplementedException(); // No variadic support atm.
        }

        // Make sure each parameter is satisfied.
        for (int i = 0; i < (Type as VarTypeFunction).Parameters.Count(); i++) {
            var argType = args[i].GetVarType();
            var paramType = parameters[i].Type;

            // Argument is being passed to template parameter.
            if (paramType.Type == VarTypeEnum.Alias && Template.Items.Where(x => x.Name.Equals((paramType as VarTypeAlias).Alias)).Count() > 0) {
                var alias = paramType as VarTypeAlias;
                if (allocatedTypes.ContainsKey((alias.Alias))) {
                    if (!allocatedTypes[alias.Alias].Equals(argType)) {
                        throw new System.NotImplementedException();
                    }
                } else {
                    allocatedTypes.Add(alias.Alias, argType);
                }
            }

            // Parameter type matches perfectly.
            else if (argType.Equals(parameters[i].Type.GetVarType())) {
                // It works, yay!
            }

            // Implict casting?
            else {
                throw new System.NotImplementedException(); // TODO: Handle possible implicit casting!
            }

        }
        return true;
    }

    // Convert a generic type into a nongeneric one.
    public VarType MakeNonGenericType(VarType type, Dictionary<string, VarType> templateParameters) {
        VarTypeAlias alias = type as VarTypeAlias;
        if (alias != null) {
            if (templateParameters.ContainsKey(alias.Alias)) {
                return templateParameters[alias.Alias];
            } else {
                return type;
            }
        } else {
            return type;
        }
    }

    // Initialize a template.
    public Function Instantiate(Dictionary<string, VarType> templateParameters) {
        var sig = Type as VarTypeFunction;
        VarType retType = MakeNonGenericType(sig.ReturnType, templateParameters);
        VarType variadicType = sig.VariadicType == null ? null : MakeNonGenericType(sig.VariadicType.Type, templateParameters);
        Variable[] parameters = new Variable[sig.Parameters.Length];
        for (int i = 0; i < parameters.Length; i++) {
            parameters[i] = new Variable(sig.Parameters[i].Name, MakeNonGenericType(sig.Parameters[i].Type, templateParameters), sig.Parameters[i].AccessFlags);
        }
        Function ret = new Function(Name, new VarTypeFunction(retType, variadicType == null ? null : new Variable(sig.VariadicType.Name, variadicType, sig.VariadicType.AccessFlags), parameters), Attributes);
        ret.Definition = Definition;
        return ret;
    }

}