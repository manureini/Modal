using Xunit;
using Bunit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components;
using Blazored.Modal.Tests.Assets;
using Blazored.Modal.Services;
using System.Threading.Tasks;

namespace Blazored.Modal.Tests
{
    public class DisplayTests : BunitContext
    {
        public DisplayTests()
        {
            Services.AddScoped<NavigationManager, MockNavigationManager>();
            Services.AddBlazoredModal();

            JSInterop.Mode = JSRuntimeMode.Loose;
        }

        [Fact]
        public void ModalIsNotVisibleByDefault()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            
            // Act
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Assert
            Assert.Empty(cut.FindAll(".bm-container"));
        }

        [Fact]
        public void ModalIsVisibleWhenShowCalled()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            modalService.Show<TestComponent>();

            // Assert
            Assert.NotNull(cut.FindComponent<BlazoredModalInstance>());
        }

        [Fact]
        public void MultipleModalsAreVisibleWhenShowCalledMultipleTimes()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            modalService.Show<TestComponent>();
            modalService.Show<TestComponent>();

            // Assert
            Assert.Equal(2, cut.FindAll(".bm-container").Count);
        }

        [Fact]
        public void ModalHidesWhenCloseCalled()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            var options = new ModalOptions
            {
                AnimationType = ModalAnimationType.None
            };
            modalService.Show<TestComponent>("", options);
            Assert.Single(cut.FindAll(".bm-container"));

            var closeButton = cut.Find(".test-component__close-button");
            closeButton.Click();

            // Assert
            Assert.Empty(cut.FindAll(".bm-container"));
        }

        [Fact]
        public void ModalHidesWhenCancelCalled()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            var options = new ModalOptions
            {
                AnimationType = ModalAnimationType.None
            };
            modalService.Show<TestComponent>("", options);
            Assert.Single(cut.FindAll(".bm-container"));

            var closeButton = cut.Find(".bm-close");
            closeButton.Click();

            // Assert
            Assert.Empty(cut.FindAll(".bm-container"));
        }

        [Fact]
        public void ModalHidesWhenReferenceCloseCalled()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            var options = new ModalOptions
            {
                AnimationType = ModalAnimationType.None
            };
            var modalReferece = modalService.Show<TestComponent>("", options);
            Assert.Single(cut.FindAll(".bm-container"));

            modalReferece.Close();

            // Assert
            Assert.Empty(cut.FindAll(".bm-container"));
        }
        
        [Fact]
        public async Task ModalHidesWhenEscapeKeyPressed()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));
            modalService.Show<TestComponent>();
            
            // Act
            await cut.InvokeAsync( () => cut.Instance.HandleEscapeKeyAsync());
            
            // Assert
            Assert.Empty(cut.FindAll(".bm-container"));
        }

        [Fact]
        public async Task TopMostModalHidesOnEscapeKeyPressedWhenMultipleAreVisible()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));
            modalService.Show<TestComponent>("First");
            modalService.Show<TestComponent>("Last");
            
            // Act
            await cut.InvokeAsync( () => cut.Instance.HandleEscapeKeyAsync());
            
            // Assert
            var instances = cut.FindAll(".bm-container");
            Assert.DoesNotContain("Last", cut.Find(".bm-title").InnerHtml);
            Assert.Single(instances);
        }

        [Fact]
        public void ModalRendersAccessibleAttributes()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));

            // Act
            modalService.Show<TestComponent>("Accessible Title");

            // Assert
            var dialog = cut.Find("div[role='dialog']");
            var title = cut.Find(".bm-title");
            var titleId = title.GetAttribute("id");

            Assert.False(string.IsNullOrWhiteSpace(titleId));
            Assert.Equal(titleId, dialog.GetAttribute("aria-labelledby"));

            var closeButton = cut.Find(".bm-close");
            Assert.Equal("Close", closeButton.GetAttribute("aria-label"));

            var closeSpan = closeButton.QuerySelector("span");
            Assert.NotNull(closeSpan);
            Assert.Equal("true", closeSpan.GetAttribute("aria-hidden"));
        }

        [Fact]
        public void ModalClosedEventFiresOnlyOnceOnClose()
        {
            // Arrange
            var modalService = Services.GetService<IModalService>();
            var cut = Render<BlazoredModal>(ps => ps.AddCascadingValue(modalService!));
            var closeCount = 0;
            cut.Instance.OnModalClosed += () => closeCount++;

            var options = new ModalOptions { AnimationType = ModalAnimationType.None };
            var modalRef = modalService.Show<TestComponent>("Test", options);

            // Act
            modalRef.Close();

            // Assert
            Assert.Equal(1, closeCount);
        }

        [Fact]
        public void ModalResultCancelPreservesPayloadType()
        {
            // Act
            var cancelResult = ModalResult.Cancel("Payload String");

            // Assert
            Assert.True(cancelResult.Cancelled);
            Assert.Equal("Payload String", cancelResult.Data);
            Assert.Equal(typeof(string), cancelResult.DataType);
        }
    }
}