from trading.money.contract import Contract


class Transaction:
    SELL = "SELL"
    BUY = "BUY"

    @staticmethod
    def _new(_type, pair, timestamp, amount, price, fee):
        return {
            'type': _type,
            'pair': pair,
            'timestamp': timestamp,
            'amount': amount,
            'price': price,
            'fee': fee
        }

    @classmethod
    def new_sell(cls, pair, timestamp, amount, price, fee):
        return Transaction._new(cls.SELL, pair, timestamp, amount, price, fee)

    @classmethod
    def new_buy(cls, pair, timestamp, amount, price, fee):
        return Transaction._new(cls.BUY, pair, timestamp, amount, price, fee)

    @classmethod
    def gained_contract(cls, transaction):
        _type = transaction['type']
        pair = transaction['pari']
        if _type == cls.SELL:
            return Contract.new(pair['priceContract'], transaction['value'] * transaction['price'])
        elif _type == cls.BUY:
            pass

    @classmethod
    def subtracted_contract(cls, transaction):
        _type = transaction['type']
        if _type == cls.SELL:
            pass
        elif _type == cls.BUY:
            pass


class SellTransaction(Transaction):
    def gained_contract(self):
        return self.amount.value * self.price

    def subtracted_contract(self):
        return self.amount


class BuyTransaction(Transaction):
    def gained_contract(self):
        return self.amount

    def subtracted_contract(self):
        return self.amount.value * self.price
