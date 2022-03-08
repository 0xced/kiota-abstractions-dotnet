// ------------------------------------------------------------------------------
//  Copyright (c) Microsoft Corporation.  All Rights Reserved.  Licensed under the MIT License.  See License in the project root for license information.
// ------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;

namespace Microsoft.Kiota.Abstractions.Serialization
{
    /// <summary>
    ///  This factory holds a list of all the registered factories for the various types of nodes.
    /// </summary>
    public class ParseNodeFactoryRegistry : IParseNodeFactory
    {
        /// <summary>
        /// The valid content type for the <see cref="ParseNodeFactoryRegistry"/>
        /// </summary>
        public string ValidContentType
        {
            get
            {
                throw new InvalidOperationException("The registry supports multiple content types. Get the registered factory instead.");
            }
        }
        /// <summary>
        /// Default singleton instance of the registry to be used when registring new factories that should be available by default.
        /// </summary>
        public static readonly ParseNodeFactoryRegistry DefaultInstance = new();
        /// <summary>
        /// List of factories that are registered by content type.
        /// </summary>
        public Dictionary<string, IParseNodeFactory> ContentTypeAssociatedFactories { get; set; } = new Dictionary<string, IParseNodeFactory>();
        /// <summary>
        /// Get the <see cref="IParseNode"/> instance that is the root of the content
        /// </summary>
        /// <param name="contentType">The content type of the stream</param>
        /// <param name="content">The <see cref="Stream"/> to parse</param>
        /// <returns></returns>
        public IParseNode GetRootParseNode(string contentType, Stream content)
        {
            if(string.IsNullOrEmpty(contentType))
                throw new ArgumentNullException(nameof(contentType));
            _ = content ?? throw new ArgumentNullException(nameof(content));

            if(ContentTypeAssociatedFactories.ContainsKey(contentType))
                return ContentTypeAssociatedFactories[contentType].GetRootParseNode(contentType, content);
            else
                throw new InvalidOperationException($"Content type {contentType} does not have a factory registered to be parsed");
        }
    }
}
