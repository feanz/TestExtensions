using TestExtensions.FluentBDD;
using Xunit;

namespace TestExtensions.Tests
{
	[Story(
	Title = "Account holder withdraws cash",
	AsA = "As an Account Holder",
	IWant = "I want to withdraw cash from an ATM",
	SoThat = "So that I can get money when the bank is closed")]
	public class AccountHolderWithdrawsCash
	{
		private Card _card;
		private Atm _atm;

		public void GivenTheAccountBalanceIs(int balance)
		{
			_card = new Card(true, balance);
		}

		public void GivenTheCardIsDisabled()
		{
			_card = new Card(false, 100);
			_atm = new Atm(100);
		}

		public void AndTheCardIsValid()
		{
		}

		public void AndTheMachineContains(int atmBalance)
		{
			_atm = new Atm(atmBalance);
		}

		public void WhenTheAccountHolderRequests(int moneyRequest)
		{
			_atm.RequestMoney(_card, moneyRequest);
		}

		public void TheAtmShouldDispense(int dispensedMoney)
		{
			Assert.Equal(dispensedMoney, _atm.DispenseValue);
		}

		public void AndTheAccountBalanceShouldBe(int balance)
		{
			Assert.Equal(balance, _card.AccountBalance);
		}

		public void CardIsRetained(bool cardIsRetained)
		{
			Assert.Equal(cardIsRetained, _atm.CardIsRetained);
		}

		public void AndTheAtmShouldSayThereAreInsufficientFunds()
		{
			Assert.Equal(DisplayMessage.InsufficientFunds, _atm.Message);
		}

		public void AndTheAtmShouldSayTheCardHasBeenRetained()
		{
			Assert.Equal(DisplayMessage.CardIsRetained, _atm.Message);
		}

		[Fact]
		public void AccountHasInsufficientFund()
		{
			this.Given(s => s.GivenTheAccountBalanceIs(10), "Given the account balance is ${0}")
					.And(s => s.AndTheCardIsValid())
					.And(s => s.AndTheMachineContains(100), "And the machine contains enough money")
				.When(s => s.WhenTheAccountHolderRequests(20), "When the account holder requests ${0}")
				.Then(s => s.TheAtmShouldDispense(0), "Then the ATM should not dispense")
					.And(s => s.AndTheAtmShouldSayThereAreInsufficientFunds())
					.And(s => s.AndTheAccountBalanceShouldBe(10))
					.And(s => s.CardIsRetained(false), "And the card should be returned to customer")
				.Run();
		}

		[Fact]
		public void CardHasBeenDisabled()
		{
			this.Given(s => s.GivenTheCardIsDisabled())
				   .When(s => s.WhenTheAccountHolderRequests(20))
				   .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
					   .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
				.Run();
		}

		[Fact]
		public void CardHasBeenDisabledExplicitStory()
		{
			this.Given(s => s.GivenTheCardIsDisabled())
				   .When(s => s.WhenTheAccountHolderRequests(20))
				   .Then(s => s.CardIsRetained(true), "Then the ATM should retain the card")
					   .And(s => s.AndTheAtmShouldSayTheCardHasBeenRetained())
				.Run<ExplicitStory>();
		}
	}

	[Story(
		Title = "This is an explicit story",
		AsA = "Developer",
		IWant = "I want to be able to separate story and scenario declaration",
		SoThat = "So they can be in different classes")]
	public class ExplicitStory
	{
	}
}