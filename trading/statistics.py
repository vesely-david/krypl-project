import pandas as pd


class Statistics:
    def __init__(self, contractName, startAmount):
        self.contractName = contractName
        self.transactions = []
        self.startAmount = startAmount

        self.currentAmount = startAmount
        self.lastAmount = startAmount
        self.maxAmount = startAmount
        self.numOfTrades = 0
        self.numOfWins = 0
        self.totalWon = 0
        self.totalLoss = 0
        self.currentLoss = 0
        self.maxDrawdownVar = .0

    def report(self):
        cols = ['startAmount', 'numberOfTrades', 'totalProfit', 'avgProfit', 'winPercentage', 'avgWinTrade',
                'avgLossTrade', 'profitFactor', 'maxDrawdown']
        dfStats = pd.DataFrame({
            'startAmount': [self.startAmount],
            'numberOfTrades': [self.numberOfTrades()],
            'totalProfit': [self.totalProfit()],
            'avgProfit': [self.avgProfit()],
            'winPercentage': [self.winPercentage()],
            'avgWinTrade': [self.avgWinTrade()],
            'avgLossTrade': [self.avgLossTrade()],
            'profitFactor': [self.profitFactor()],
            'maxDrawdown': [self.maxDrawdown()]
        })[cols].transpose()
        dfStats.columns = [self.contractName]
        return dfStats

    def evaluate(self, transactions):
        for transaction in transactions:
            self.addTransaction(transaction)
        return self

    def addTransaction(self, transaction):
        if self.isNextTransactionOpen():
            self.openTrade(transaction)
        else:
            self.closeTrade(transaction)

    def openTrade(self, transaction):
        self.transactions.append(transaction)
        self.lastAmount = self.currentAmount
        self.currentAmount += self.contractAmount(transaction) - self.fee(transaction)

    def closeTrade(self, transaction):
        self.transactions.append(transaction)
        self.currentAmount += self.contractAmount(transaction) - self.fee(transaction)
        self.numOfTrades += 1
        self.updateMaxAmount()
        self.updateWins()
        self.updateLosses()

    def numberOfTrades(self):
        return self.numOfTrades

    def totalProfit(self):
        return self.currentAmount - self.startAmount

    def avgProfit(self):
        return float(self.totalProfit()) / self.numberOfTrades() if self.numberOfTrades() > 0 else 0

    def winPercentage(self):
        return 100. * self.numOfWins / self.numberOfTrades() if self.numberOfTrades() > 0 else 0

    def avgWinTrade(self):
        return float(self.totalWon) / self.numOfWins if self.numOfWins > 0 else 0

    def avgLossTrade(self):
        numOfLosses = self.numberOfTrades() - self.numOfWins
        return float(-self.totalLoss) / numOfLosses if numOfLosses > 0 else 0

    def profitFactor(self):
        return float(self.totalWon) / self.totalLoss if self.totalLoss > 0 else float('inf')

    def maxDrawdown(self):
        return self.maxDrawdownVar

    # ------------ Private --------------------------
    def fee(self, transaction):
        if transaction.fee.name == self.contractName:
            return transaction.fee.value
        else:
            return (transaction.fee / transaction.price).value

    def updateMaxAmount(self):
        if self.currentAmount > self.maxAmount:
            self.maxAmount = self.currentAmount

    def updateWins(self):
        if self.isLastWin():
            self.numOfWins += 1
            self.totalWon += self.currentAmount - self.lastAmount

    def updateDrawdown(self, ):
        drawdown = 100. * (self.maxAmount - self.currentAmount) / self.maxAmount
        if drawdown > self.maxDrawdownVar:
            self.maxDrawdownVar = drawdown

    def updateLosses(self):
        if self.isLastLoss():
            self.totalLoss += self.lastAmount - self.currentAmount
            self.updateDrawdown()

    def numOfTransactions(self):
        return len(self.transactions)

    def isNextTransactionOpen(self):
        return self.numOfTransactions() % 2 == 0

    def isNextTransactionClose(self):
        return not self.isNextTransactionOpen()

    def isLastWin(self):
        return self.currentAmount > self.lastAmount

    def isLastLoss(self):
        return not self.isLastWin()

    def contractAmount(self, transaction):
        gained = transaction.gained_contract()
        subtracted = transaction.subtracted_contract()
        if gained.name == self.contractName:
            return gained.value
        elif subtracted.name == self.contractName:
            return -subtracted.value

        raise ValueError('Transaction does not consist wanted contract.')