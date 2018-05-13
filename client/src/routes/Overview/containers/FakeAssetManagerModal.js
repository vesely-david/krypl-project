import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Modal, Message, Form, Icon, Label } from 'semantic-ui-react'

const getCurrency = (dataObject, exchange, currency) => {
  const ex = dataObject.find(o => o.id === exchange)
  return ex ? ex.currencies.find(o => o.id === currency) : null
}

const getExchange = (dataObject, exchange) => {
  return dataObject.find(o => o.id === exchange)
}

class FakeAssetManagerModal extends React.Component {
  constructor (props) {
    super(props)

    this.state = {
      selectedExchange: null,
      selectedCurrency: null,
      error: null,
      assetValue: '',
      assetValueMinimum: 0,
      assetValueError: false,
      showMessage: false,
      submitUnclocked: false,
      toSubmit: [],
    }
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value, assetValueError: false })
  }

  onExchangeSelect = (e, { value }) => {
    const { toSubmit } = this.state
    if (!getExchange(toSubmit, value)) {
      this.setState({ toSubmit: [...toSubmit, { id: value, currencies: [] }] })
    }
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange: value,
      selectedCurrency: null
    })
  }

  onCurrencySelect = (e, { value }) => {
    if (value === this.state.selectedCurrency) return
    const { selectedExchange, toSubmit } = this.state
    const { marketData } = this.props

    const currencyOrigin = getCurrency(marketData, selectedExchange, value)
    const currencyNow = getCurrency(toSubmit, selectedExchange, value)
    this.setState({
      assetValue: currencyNow ? currencyNow.amount : currencyOrigin ? currencyOrigin.sum : 0,
      assetValueMinimum: currencyOrigin ? currencyOrigin.sum - currencyOrigin.free : 0,
      assetValueError: false,
      selectedCurrency: value
    })
  }

  onAssetAdd = () => {
    const {
      toSubmit,
      selectedExchange,
      selectedCurrency,
      assetValue,
    } = this.state
    const {
      assets,
    } = this.props

    const currencyAsset = getCurrency(assets, selectedExchange, selectedCurrency)
    const minAmount = currencyAsset ? currencyAsset.sum - currencyAsset.free : 0
    if (!assetValue.match(/^(0\.\d+)$|^([1-9]\d*(\.\d+)?)$/) ||
      (minAmount && minAmount > parseFloat(assetValue))) {
      this.setState({
        assetValueError: true,
      })
      return
    }
    let newToSubmit = [...toSubmit]
    let currency = getCurrency(newToSubmit, selectedExchange, selectedCurrency)
    if (currency) {
      currency.amount = parseFloat(assetValue)
    } else {
      getExchange(newToSubmit, selectedExchange).currencies.push(
        { id: selectedCurrency, amount: parseFloat(assetValue), minAmount: minAmount }
      )
    }
    this.setState({
      assetValue: '',
      selectedCurrency: null,
      assetValueError: false,
      toSubmit: newToSubmit,
      assetValueMinimum: 0,
      submitUnclocked: true
    })
  }

  onAssetRemove = (currency) => {
    const {
      selectedExchange,
      toSubmit,
    } = this.state

    const newToSubmit = toSubmit.map(o => o.id === selectedExchange
      ? { ...o,
        currencies: o.currencies
        .map(p => p.id === currency
        ? { ...p, amount: p.minAmount ? p.minAmount : 0 }
        : p) }
      : o)
    this.setState({
      toSubmit: newToSubmit,
      submitUnclocked: true
    })
  }

  onSubmit = async () => {
    const {
      toSubmit,
      selectedExchange
    } = this.state

    const assetsToSubmit = getExchange(toSubmit, selectedExchange).currencies
      .map(o => ({ currency: o.id, amount: o.amount }))
    const error = await this.props.submitAssets(selectedExchange, assetsToSubmit)
    if (error !== null) this.setState({ error: error })
    else this.setState({ selectedExchange: null })
    this.setState({ showMessage: true })
  }

  cleanForm = () => {
    this.setState({
      selectedExchange: null,
      selectedCurrency: null,
      error: null,
      assetValueMinimum: 0,
      assetValueError: false,
      showMessage: false,
      finalObject: {},
      submitUnclocked: false
    })
  }

  render () {
    const {
      toSubmit,
      selectedExchange,
      selectedCurrency,
      showMessage,
      error,
      assetValue,
      assetValueError,
      assetValueMinimum,
      submitUnclocked
    } = this.state
    const {
      color,
      marketData,
      marketDataFetching,
      submitAssetsFetching,
    } = this.props

    const possibleExchanges = marketData
      ? marketData.map(o => ({ key: o.id, text: o.name, value: o.id }))
      : []
    const possibleCurrencies = selectedExchange
      ? marketData.find(o => o.id === selectedExchange).currencies
        .map(o => ({ key: o.id, text: o.id, value: o.id }))
      : []

    return (
      <Modal trigger={
        <Button
          disabled={marketDataFetching || possibleExchanges.length === 0}
          color={color}
          loading={marketDataFetching}
        >
          Manage assets
        </Button>
        } closeIcon onClose={this.cleanForm} size='small' onOpen={this.onModalOpen}>
        <Header icon='money' content='Asset Management' />
        <Modal.Content>
          <div className='newStrategyModalContent'>
            <Form widths='equal' loading={submitAssetsFetching}>
              <Form.Group>
                <Form.Dropdown
                  fluid
                  search
                  selection
                  label='Exchange'
                  placeholder='Exchange'
                  name='exchange'
                  width={5}
                  value={selectedExchange}
                  options={possibleExchanges}
                  onChange={this.onExchangeSelect} />
                <Form.Dropdown
                  fluid
                  search
                  selection
                  label='Currency'
                  placeholder='Currency'
                  name='currency'
                  width={4}
                  disabled={selectedExchange == null}
                  value={selectedCurrency}
                  options={possibleCurrencies}
                  onChange={this.onCurrencySelect} />
                <Form.Input
                  type='number'
                  disabled={selectedCurrency === null}
                  fluid
                  error={assetValueError}
                  label={`Value${assetValueMinimum ? ` (Minimum: ${assetValueMinimum})` : ''}`}
                  value={assetValue || ''}
                  name='assetValue'
                  width={5}
                  onChange={this.onInputChange} />
                <Form.Button
                  disabled={assetValue === ''}
                  onClick={this.onAssetAdd}color='green'
                  width={2}
                  className='modalButton'>
                  <Icon name='checkmark' size='large' />
                </Form.Button>
              </Form.Group>
            </Form>
            <div style={{ minHeight: '30px' }}>
              {selectedExchange && getExchange(toSubmit, selectedExchange).currencies
                .filter(o => o.amount > 0)
                .map(o => (
                  <Label className='currencyLabel' color={color} key={o.id}>
                    {`${o.id} ${o.amount}`}
                    <Label.Detail>{o.taken}</Label.Detail>
                    { o.minAmount < o.amount &&
                      <Icon name='close' onClick={() => this.onAssetRemove(o.id)} />}
                  </Label>
                ))}
            </div>
          </div>
          <Message
            success={showMessage && !error}
            error={showMessage && error}
            hidden={!showMessage}
            header={error ? `Insufficient funds for:\n ${error}` : 'Success'}
          />
        </Modal.Content>
        <Modal.Actions>
          <Button
            disabled={!submitUnclocked}
            onClick={this.onSubmit}
            color={color}
            className='modalButton'>
            Submit Assets
          </Button>
        </Modal.Actions>
      </Modal>
    )
  }
}

FakeAssetManagerModal.propTypes = {
  color: PropTypes.string,
  assets: PropTypes.array,
  submitAssets: PropTypes.func,
  submitAssetsFetching: PropTypes.bool,
  marketData: PropTypes.array,
  marketDataFetching: PropTypes.bool,
}

export default FakeAssetManagerModal
