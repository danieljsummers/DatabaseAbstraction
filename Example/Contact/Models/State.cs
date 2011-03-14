namespace com.codeplex.dbabstraction.Contact.Models {

    /// <summary>
    /// This represents a state
    /// </summary>
    public struct State {

        /// <summary>
        /// The ID for this state
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// The postal abbreviation of this state
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// The full name of this state
        /// </summary>
        public string Name { get; set; }
    }
}