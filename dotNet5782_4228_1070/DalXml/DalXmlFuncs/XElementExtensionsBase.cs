//namespace Dal
//{
//    public static class XElementExtensionsBase
//    {

        
//        public void GetElementValueShouldReturnValueOfIntegerElementAsInteger()
//        {
//            const int expectedValue = 5;
//            const string elementName = "intProp";
//            var xElement = new XElement("name");
//            var integerElement = new XElement(elementName) { Value = expectedValue.ToString() };
//            xElement.Add(integerElement);

//            int value = XElementExtensions.GetElementValue<int>(xElement, elementName);

//            Assert.AreEqual(expectedValue, value, "Expected integer value was not returned from element.");
//        }
//    }
//}