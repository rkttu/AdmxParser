using AdmxParser.Models;
using AdmxParser.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;

namespace AdmxParser
{
    /// <summary>
    /// Provides methods for parsing ADMX models.
    /// </summary>
    public static class AdmxModeller
    {
        private static readonly Lazy<Regex> _refRegexFactory = new Lazy<Regex>(
            () => new Regex(@"\$\((?<ResourceType>[^.]+)\.(?<ResourceKey>[^\)]+)\)", RegexOptions.Compiled | RegexOptions.IgnoreCase),
            LazyThreadSafetyMode.None);

        /// <summary>
        /// Parses the ADMX models in the specified directory.
        /// </summary>
        /// <param name="admxDirectory">The ADMX directory to parse.</param>
        /// <returns>A collection of parsed policy definition items.</returns>
        public static ParsedPolicyDefinitionItemCollection ParseModels(this AdmxDirectory admxDirectory)
        {
            var collection = new ParsedPolicyDefinitionItemCollection();

            foreach (var targetAdmx in admxDirectory.LoadedAdmxContents)
            {
                var baseResource = targetAdmx.LoadedAdmlResources.FirstOrDefault(x => string.Equals(x.Key.Name, "en-US", StringComparison.OrdinalIgnoreCase)).Value;
                if (baseResource == null)
                    baseResource = targetAdmx.LoadedAdmlResources.FirstOrDefault().Value;

                var stringKeys = baseResource != null ? baseResource.StringKeys : Array.Empty<string>();
                var stringTable = baseResource != null ? baseResource.StringTable : new ReadOnlyDictionary<string, string>(new Dictionary<string, string>());
                var presentations = baseResource != null ? baseResource.PresentationTable : Array.Empty<PolicyPresentation>();

                var policies = new ParsedPolicyItemCollection();

                foreach (var eachPolicy in targetAdmx.Policies)
                {
                    var key = eachPolicy.key;
                    var valueName = eachPolicy.valueName;

                    var policy = new ParsedPolicyItem()
                    {
                        Name = eachPolicy.name,
                        IsMachinePolicy = eachPolicy.@class == PolicyClass.Machine || eachPolicy.@class == PolicyClass.Both,
                        IsUserPolicy = eachPolicy.@class == PolicyClass.User || eachPolicy.@class == PolicyClass.Both,
                    };

                    var eachPolicyPresentation = default(PolicyPresentation);
                    if (eachPolicy.presentation != null)
                    {
                        var resourceKeyRegex = _refRegexFactory.Value;
                        var presentationMatch = resourceKeyRegex.Match(eachPolicy.presentation);
                        if (presentationMatch.Success &&
                            string.Equals("presentation", presentationMatch.Groups["ResourceType"].Value, StringComparison.OrdinalIgnoreCase))
                        {
                            eachPolicyPresentation = presentations
                                .FirstOrDefault(x => string.Equals(x.id, presentationMatch.Groups["ResourceKey"].Value, StringComparison.Ordinal));
                        }
                    }

                    var enabledValue = eachPolicy.enabledValue;
                    if (enabledValue != null)
                        policy.EnableItems.Add(enabledValue.ToRegistryItem(key, valueName, /* isPrefix */ false));

                    if (eachPolicy.enabledList != null)
                    {
                        var enabledListKey = eachPolicy.enabledList.defaultKey;
                        enabledListKey = string.IsNullOrWhiteSpace(enabledListKey) ? key : enabledListKey;

                        foreach (var eachEnabledItem in eachPolicy.enabledList.item)
                        {
                            var eachEnabledKey = eachEnabledItem.key;
                            eachEnabledKey = string.IsNullOrWhiteSpace(eachEnabledKey) ? enabledListKey : eachEnabledKey;
                            policy.EnableItems.Add(eachEnabledItem.value.ToRegistryItem(eachEnabledKey, eachEnabledItem.valueName, /* isPrefix */ false));
                        }
                    }

                    var disabledValue = eachPolicy.disabledValue;
                    if (disabledValue != null)
                        policy.DisableItems.Add(disabledValue.ToRegistryItem(key, valueName, /* isPrefix */ false));

                    if (eachPolicy.disabledList != null)
                    {
                        var disabledListKey = eachPolicy.disabledList.defaultKey;
                        disabledListKey = string.IsNullOrWhiteSpace(disabledListKey) ? key : disabledListKey;

                        foreach (var eachDisabledItem in eachPolicy.disabledList.item)
                        {
                            var eachDisabledKey = eachDisabledItem.key;
                            eachDisabledKey = string.IsNullOrWhiteSpace(eachDisabledKey) ? disabledListKey : eachDisabledKey;
                            policy.DisableItems.Add(eachDisabledItem.value.ToRegistryItem(eachDisabledKey, eachDisabledItem.valueName, /* isPrefix */ false));
                        }
                    }

                    if (eachPolicy.elements != null)
                    {
                        foreach (var eachElement in eachPolicy.elements)
                        {
                            var element = default(ParsedPolicyElementItem);

                            switch (eachElement)
                            {
                                case BooleanElement be:
                                    var beKey = be.key;
                                    beKey = string.IsNullOrWhiteSpace(beKey) ? key : beKey;

                                    var boolean = new ParsedBooleanElementItem()
                                    {
                                        Id = be.id,
                                        ClientExtension = be.clientExtension,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = beKey,
                                            ValueName = be.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(int),
                                        },
                                    };

                                    var checkBox = eachPolicyPresentation?.GetCheckBox(be.id);
                                    if (checkBox != null)
                                        boolean.DefaultValue = checkBox.defaultChecked;

                                    if (be.trueValue != null)
                                        boolean.TrueValue = be.trueValue.ToCLRType();

                                    if (be.trueList != null)
                                    {
                                        var trueListKey = be.trueList.defaultKey;
                                        trueListKey = string.IsNullOrWhiteSpace(trueListKey) ? beKey : trueListKey;

                                        foreach (var eachTrueItem in be.trueList.item)
                                        {
                                            var eachTrueKey = eachTrueItem.key;
                                            eachTrueKey = string.IsNullOrWhiteSpace(eachTrueKey) ? trueListKey : eachTrueKey;
                                            boolean.TrueItems.Add(eachTrueItem.value.ToRegistryItem(eachTrueKey, eachTrueItem.valueName, false));
                                        }
                                    }

                                    if (be.falseValue != null)
                                        boolean.FalseValue = be.falseValue.ToCLRType();

                                    if (be.falseList != null)
                                    {
                                        var falseListKey = be.falseList.defaultKey;
                                        falseListKey = string.IsNullOrWhiteSpace(falseListKey) ? beKey : falseListKey;

                                        foreach (var eachFalseItem in be.falseList.item)
                                        {
                                            var eachFalseKey = eachFalseItem.key;
                                            eachFalseKey = string.IsNullOrWhiteSpace(eachFalseKey) ? falseListKey : eachFalseKey;
                                            boolean.FalseItems.Add(eachFalseItem.value.ToRegistryItem(eachFalseKey, eachFalseItem.valueName, false));
                                        }
                                    }

                                    element = boolean;
                                    break;
                                case DecimalElement de:
                                    var deKey = de.key;
                                    deKey = string.IsNullOrWhiteSpace(deKey) ? key : deKey;
                                    var @decimal = new ParsedDecimalElementItem()
                                    {
                                        Id = de.id,
                                        ClientExtension = de.clientExtension,
                                        MaxValue = unchecked((int)de.maxValue),
                                        MinValue = unchecked((int)de.minValue),
                                        Required = de.required,
                                        Soft = de.soft,
                                        StoreAsText = de.storeAsText,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = deKey,
                                            ValueName = de.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(int),
                                        },
                                    };

                                    var decimalTextBox = eachPolicyPresentation?.GetDecimalTextBox(de.id);
                                    if (decimalTextBox != null)
                                        @decimal.DefaultValue = unchecked((int)decimalTextBox.defaultValue);

                                    element = @decimal;
                                    break;
                                case EnumerationElement ee:
                                    var eeKey = ee.key;
                                    eeKey = string.IsNullOrWhiteSpace(eeKey) ? key : eeKey;
                                    var @enum = new ParsedEnumerationElementItem()
                                    {
                                        Id = ee.id,
                                        ClientExtension = ee.clientExtension,
                                        Required = ee.required,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = eeKey,
                                            ValueName = ee.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(object),
                                        },
                                    };

                                    foreach (var eachMember in ee.item)
                                    {
                                        if (eachMember == null)
                                            continue;
                                        if (eachMember.value == null)
                                            continue;

                                        var memberKey = eachMember.value.ToCLRType();
                                        var memberRegistryItems = new List<ParsedRegistryFixedItem>();

                                        if (eachMember.valueList != null)
                                        {
                                            var eachEmKey = eachMember.valueList.defaultKey;
                                            eachEmKey = string.IsNullOrWhiteSpace(eachEmKey) ? eeKey : eachEmKey;

                                            foreach (var eachMemberValue in eachMember.valueList.item)
                                            {
                                                var eachMvKey = eachMemberValue.key;
                                                eachMvKey = string.IsNullOrWhiteSpace(eachMvKey) ? eachEmKey : eachMvKey;
                                                memberRegistryItems.Add(eachMemberValue.value.ToRegistryItem(eachMvKey, eachMemberValue.valueName, false));
                                            }
                                        }

                                        var member = new ParsedEnumerationMemberItem()
                                        {
                                            MemberValue = eachMember?.value?.ToCLRType(),
                                            DisplayNameKey = eachMember?.displayName,
                                            RegistryItems = memberRegistryItems,
                                        };

                                        var regex = _refRegexFactory.Value;
                                        var match = regex.Match(member.DisplayNameKey ?? string.Empty);

                                        if (match.Success)
                                            member.MemberName = match.Groups["ResourceKey"].Value;
                                        else
                                            member.MemberName = $"Item{@enum.EnumMemberItems.Count + 1}";

                                        @enum.EnumMemberItems.Add(member);
                                    }

                                    var dropdownList = eachPolicyPresentation?.GetDropdownList(ee.id);
                                    if (dropdownList != null && dropdownList.defaultItemSpecified)
                                        @enum.DefaultValue = @enum.EnumMemberItems.ElementAtOrDefault(unchecked((int)dropdownList.defaultItem))?.MemberValue;

                                    element = @enum;
                                    break;
                                case ListElement le:
                                    var leKey = le.key;
                                    leKey = string.IsNullOrWhiteSpace(leKey) ? key : leKey;
                                    var list = new ParsedListElementItem()
                                    {
                                        Id = le.id,
                                        ClientExtension = le.clientExtension,
                                        Additive = le.additive,
                                        Expandable = le.expandable,
                                        ExplicitValue = le.explicitValue,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = leKey,
                                            ValueName = le.valuePrefix,
                                            IsPrefix = true,
                                            ValueType = typeof(string[]),
                                        },
                                    };

                                    element = list;
                                    break;
                                case LongDecimalElement lde:
                                    var ldeKey = lde.key;
                                    ldeKey = string.IsNullOrWhiteSpace(ldeKey) ? key : ldeKey;
                                    var longDecimal = new ParsedLongDecimalElementItem()
                                    {
                                        Id = lde.id,
                                        ClientExtension = lde.clientExtension,
                                        MaxValue = unchecked((long)lde.maxValue),
                                        MinValue = unchecked((long)lde.minValue),
                                        Required = lde.required,
                                        Soft = lde.soft,
                                        StoreAsText = lde.storeAsText,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = ldeKey,
                                            ValueName = lde.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(long),
                                        },
                                    };

                                    var longDecimalTextBox = eachPolicyPresentation?.GetLongDecimalTextBox(lde.id);
                                    if (longDecimalTextBox != null)
                                        longDecimal.DefaultValue = longDecimalTextBox.defaultValue;

                                    element = longDecimal;
                                    break;
                                case multiTextElement mte:
                                    var mteKey = mte.key;
                                    ldeKey = string.IsNullOrWhiteSpace(mteKey) ? key : mteKey;
                                    var multiText = new ParsedMultiTextElementItem()
                                    {
                                        Id = mte.id,
                                        ClientExtension = mte.clientExtension,
                                        MaxLength = (int)mte.maxLength,
                                        MaxStrings = (int)mte.maxStrings,
                                        Required = mte.required,
                                        Soft = mte.soft,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = ldeKey,
                                            ValueName = mte.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(string[]),
                                        },
                                    };

                                    element = multiText;
                                    break;
                                case TextElement te:
                                    var teKey = te.key;
                                    ldeKey = string.IsNullOrWhiteSpace(teKey) ? key : teKey;
                                    var text = new ParsedTextElementItem()
                                    {
                                        Id = te.id,
                                        ClientExtension = te.clientExtension,
                                        MaxLength = (int)te.maxLength,
                                        Required = te.required,
                                        Expandable = te.expandable,
                                        Soft = te.soft,

                                        Item = new ParsedRegistryElementItem()
                                        {
                                            Key = ldeKey,
                                            ValueName = te.valueName,
                                            IsPrefix = false,
                                            ValueType = typeof(string),
                                        },
                                    };

                                    var textBox = eachPolicyPresentation?.GetTextBox(te.id);
                                    if (textBox != null)
                                        text.DefaultValue = textBox.defaultValue;
                                    else
                                    {
                                        var comboBox = eachPolicyPresentation?.GetComboBox(te.id);
                                        if (comboBox != null)
                                            text.DefaultValue = comboBox.@default;
                                    }

                                    element = text;
                                    break;
                            }

                            if (element == null)
                                continue;

                            policy.Elements.Add(element);
                        }
                    }

                    // Reset Items
                    var resetItems = new List<ParsedRegistryRemoveItem>();
                    foreach (var eachItem in policy.EnableItems)
                        resetItems.Add(new ParsedRegistryRemoveItem { Key = eachItem.Key, ValueName = eachItem.ValueName, IsPrefix = false, });
                    foreach (var eachItem in policy.DisableItems)
                        resetItems.Add(new ParsedRegistryRemoveItem { Key = eachItem.Key, ValueName = eachItem.ValueName, IsPrefix = false, });
                    foreach (var eachElement in policy.Elements)
                    {
                        switch (eachElement)
                        {
                            case ParsedBooleanElementItem be:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = be.Item.Key, ValueName = be.Item.ValueName, IsPrefix = be.Item.IsPrefix, });
                                foreach (var eachTrueItem in be.TrueItems)
                                    resetItems.Add(new ParsedRegistryRemoveItem { Key = eachTrueItem.Key, ValueName = eachTrueItem.ValueName, IsPrefix = false, });
                                foreach (var eachFalseItem in be.FalseItems)
                                    resetItems.Add(new ParsedRegistryRemoveItem { Key = eachFalseItem.Key, ValueName = eachFalseItem.ValueName, IsPrefix = false, });
                                break;
                            case ParsedDecimalElementItem de:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = de.Item.Key, ValueName = de.Item.ValueName, IsPrefix = de.Item.IsPrefix, });
                                break;
                            case ParsedEnumerationElementItem ee:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = ee.Item.Key, ValueName = ee.Item.ValueName, IsPrefix = ee.Item.IsPrefix, });
                                foreach (var eachEnumItem in ee.EnumMemberItems)
                                    resetItems.AddRange(eachEnumItem.RegistryItems.Select(x => new ParsedRegistryRemoveItem { Key = x.Key, ValueName = x.ValueName, IsPrefix = false, }));
                                break;
                            case ParsedListElementItem le:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = le.Item.Key, ValueName = le.Item.ValueName, IsPrefix = le.Item.IsPrefix, });
                                break;
                            case ParsedLongDecimalElementItem lde:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = lde.Item.Key, ValueName = lde.Item.ValueName, IsPrefix = lde.Item.IsPrefix, });
                                break;
                            case ParsedMultiTextElementItem mte:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = mte.Item.Key, ValueName = mte.Item.ValueName, IsPrefix = mte.Item.IsPrefix, });
                                break;
                            case ParsedTextElementItem te:
                                resetItems.Add(new ParsedRegistryRemoveItem { Key = te.Item.Key, ValueName = te.Item.ValueName, IsPrefix = te.Item.IsPrefix, });
                                break;
                        }
                    }

                    policy.ResetItems = resetItems
                        .Select(x => new { Path = $"{x.Key}\\{x.ValueName}", Item = x })
                        .GroupBy(x => x.Path)
                        .Select(g => g.First().Item)
                        .ToList();

                    var stringRefRegex = _refRegexFactory.Value;

                    policy.DisplayName = eachPolicy.name;
                    var displayNameMatch = stringRefRegex.Match(eachPolicy.displayName);
                    if (displayNameMatch.Success)
                    {
                        var resourceType = displayNameMatch.Groups["ResourceType"].Value;
                        var resourceKey = displayNameMatch.Groups["ResourceKey"].Value;

                        if (string.Equals("string", resourceType, StringComparison.OrdinalIgnoreCase) &&
                            stringTable.ContainsKey(resourceKey))
                        {
                            policy.DisplayName = stringTable[resourceKey];
                        }
                    }

                    policy.ExplainText = $"This policy class reads and writes {eachPolicy.valueName} in {eachPolicy.key} registry key.";
                    var explainMatch = stringRefRegex.Match(eachPolicy.explainText);
                    if (explainMatch.Success)
                    {
                        var resourceType = explainMatch.Groups["ResourceType"].Value;
                        var resourceKey = explainMatch.Groups["ResourceKey"].Value;

                        if (string.Equals("string", resourceType, StringComparison.OrdinalIgnoreCase) &&
                            stringTable.ContainsKey(resourceKey))
                        {
                            policy.ExplainText = stringTable[resourceKey];
                        }
                    }

                    policies.Add(policy);
                }

                var definition = new ParsedPolicyDefinitionItem()
                {
                    Namespace = targetAdmx.TargetNamespace.@namespace,
                    Prefix = targetAdmx.TargetNamespace.prefix,
                    Policies = policies,
                    BaseStringTable = stringTable,
                };

                collection.Add(definition);
            }

            return collection;
        }
    }
}
