﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CoolBro.Resources {
    using System;
    
    
    /// <summary>
    ///   Класс ресурса со строгой типизацией для поиска локализованных строк и т.д.
    /// </summary>
    // Этот класс создан автоматически классом StronglyTypedResourceBuilder
    // с помощью такого средства, как ResGen или Visual Studio.
    // Чтобы добавить или удалить член, измените файл .ResX и снова запустите ResGen
    // с параметром /str или перестройте свой проект VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Возвращает кэшированный экземпляр ResourceManager, использованный этим классом.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("CoolBro.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Перезаписывает свойство CurrentUICulture текущего потока для всех
        ///   обращений к ресурсу с помощью этого класса ресурса со строгой типизацией.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Команда не найдена🤔.
        /// </summary>
        internal static string CommandNotFound {
            get {
                return ResourceManager.GetString("CommandNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на У вас пока нет тикетов.
        /// </summary>
        internal static string DontHaveTicketsYet {
            get {
                return ResourceManager.GetString("DontHaveTicketsYet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на ✨Введите сообщение которое хотите отправить ниже:.
        /// </summary>
        internal static string EnterYouMessage {
            get {
                return ResourceManager.GetString("EnterYouMessage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 😁 Привет, {0}! Ты попал в бота для обращения к кулеру. Ориентируйся по кнопкам ниже..
        /// </summary>
        internal static string MainMenu {
            get {
                return ResourceManager.GetString("MainMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Функция недоступна..
        /// </summary>
        internal static string NotEnoughPrivileges {
            get {
                return ResourceManager.GetString("NotEnoughPrivileges", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Здесь вы можете управлять вашими обращениями, либо написать новое:.
        /// </summary>
        internal static string SupportMenu {
            get {
                return ResourceManager.GetString("SupportMenu", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Вы уверены что хотите удалить обращение?.
        /// </summary>
        internal static string TicketDeleteConfirmed {
            get {
                return ResourceManager.GetString("TicketDeleteConfirmed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 👀 Тикет №{0}
        ///
        ///⚫️Текст тикета: {1}
        ///
        ///🎇 Прочитано: {2}
        ///
        ///🕘 Дата создания: {3}.
        /// </summary>
        internal static string TicketInfo {
            get {
                return ResourceManager.GetString("TicketInfo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 🎇Обращение успешно создано. Вы можете вернуться назад:.
        /// </summary>
        internal static string TicketIsCreated {
            get {
                return ResourceManager.GetString("TicketIsCreated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Тикет не найден😢.
        /// </summary>
        internal static string TicketNotFound {
            get {
                return ResourceManager.GetString("TicketNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на ✅Тикет успешно удалён.
        /// </summary>
        internal static string TicketSuccesfullyDeleted {
            get {
                return ResourceManager.GetString("TicketSuccesfullyDeleted", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на 😔Вы можете создавать тикеты только раз в 10 часов. 
        ///
        ///Последний тикет был создан {0}..
        /// </summary>
        internal static string TicketTimedOut {
            get {
                return ResourceManager.GetString("TicketTimedOut", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Ищет локализованную строку, похожую на Ваши тикеты:.
        /// </summary>
        internal static string YourTickets {
            get {
                return ResourceManager.GetString("YourTickets", resourceCulture);
            }
        }
    }
}
