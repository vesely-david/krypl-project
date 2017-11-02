from enum import Enum


class Transaction:
    Type = Enum('Type', 'buy sell')

    def __init__(self, timestamp, amount, price, transactionType, fee):
        self.timestamp = timestamp
        self.amount = amount
        self.price = price
        self.type = transactionType
        self.fee = fee
