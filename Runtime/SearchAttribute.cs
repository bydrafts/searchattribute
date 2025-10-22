using UnityEngine;

namespace BlueGravity {
	public abstract class SearchAttribute : PropertyAttribute {
		public virtual ISearchSettings Settings { get; }
		public bool Lock { get; }
		public SearchAttribute(ISearchSettings settings) => Settings = settings;
		public SearchAttribute(ISearchSettings settings, bool @lock) {
			Settings = settings;
			Lock = @lock;
		}
	}
}