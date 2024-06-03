using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace AdmxParser.Models.Adml
{
    /// <summary>
    /// Represents a presentation entity.
    /// </summary>
    public class Presentation : AdmlData
    {
        /// <summary>
        /// Creates a new instance of the <see cref="Presentation"/> class.
        /// </summary>
        /// <param name="parent">
        /// The parent resource of the entity.
        /// </param>
        /// <param name="sourceElement">
        /// The source element of the entity.
        /// </param>
        protected Presentation(AdmlResource parent, XElement sourceElement) :
            base(parent, sourceElement)
        {
            _id = sourceElement.Attribute("id")?.Value;

            var pathPrefix = Parent.PathPrefix;
            var nsManager = Parent.NamespaceManager;

            var childElements = new List<AdmlPresentationControl>();
            foreach (var eachPresentationElem in sourceElement.Elements())
            {
                switch (eachPresentationElem.Name.LocalName.ToUpperInvariant())
                {
                    case "TEXT":
                        childElements.Add(parent.CreateAdmlData<TextPresentation>(eachPresentationElem));
                        break;

                    case "LONGDECIMALTEXTBOX":
                        childElements.Add(parent.CreateAdmlData<LongDecimalTextBoxPresentation>(eachPresentationElem));
                        break;

                    case "DECIMALTEXTBOX":
                        childElements.Add(parent.CreateAdmlData<DecimalTextBoxPresentation>(eachPresentationElem));
                        break;

                    case "TEXTBOX":
                        childElements.Add(parent.CreateAdmlData<TextBoxPresentation>(eachPresentationElem));
                        break;

                    case "MULTITEXTBOX":
                        childElements.Add(parent.CreateAdmlData<MultiTextBoxPresentation>(eachPresentationElem));
                        break;

                    case "CHECKBOX":
                        childElements.Add(parent.CreateAdmlData<CheckBoxPresentation>(eachPresentationElem));
                        break;

                    case "COMBOBOX":
                        childElements.Add(parent.CreateAdmlData<ComboBoxPresentation>(eachPresentationElem));
                        break;

                    case "DROPDOWNLIST":
                        childElements.Add(parent.CreateAdmlData<DropDownListPresentation>(eachPresentationElem));
                        break;

                    case "LISTBOX":
                        childElements.Add(parent.CreateAdmlData<ListBoxPresentation>(eachPresentationElem));
                        break;

                    default:
                        throw new Exception($"Unknown presentation control: {eachPresentationElem.Name.LocalName}");
                }
            }

            _childElements = childElements.ToArray();
        }

        private readonly string _id;
        private readonly AdmlPresentationControl[] _childElements;

        /// <summary>
        /// Gets the ID of the entity.
        /// </summary>
        public string Id => _id;

        /// <summary>
        /// Gets the child elements of the entity.
        /// </summary>
        public IReadOnlyList<AdmlPresentationControl> ChildElements => _childElements;
    }
}
