import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Modal, Message, Form, Icon, Label } from 'semantic-ui-react'

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
      finalObject: {},
      submitUnclocked: false
    }
  }

  onModalOpen = () => {
    let newFinalObject = {}

    this.props.assetOptions.forEach((exchange) => {
      if (!newFinalObject[exchange.value]) newFinalObject[exchange.value] = {}
      exchange.currencies.forEach(currency => {
        newFinalObject[exchange.value][currency.value] = {}
        newFinalObject[exchange.value][currency.value].value = 0
        newFinalObject[exchange.value][currency.value].min = 0
        newFinalObject[exchange.value][currency.value].text = currency.text
        newFinalObject[exchange.value][currency.value].currValue = currency.value
      })
    })

    this.props.assets.forEach((asset) => {
      asset.assets.forEach((currency) => {
        newFinalObject[asset.value][currency.value].value = currency.sum
        newFinalObject[asset.value][currency.value].min = currency.sum - currency.free
      })
    })

    this.setState({ finalObject: newFinalObject })
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value, assetValueError: false })
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange: value,
      selectedCurrency: null
    })
  }

  onCurrencySelect = (e, { value }) => {
    if (value === this.state.selectedCurrency) return
    const { finalObject, selectedExchange } = this.state
    this.setState({
      assetValue: finalObject[selectedExchange][value].value,
      assetValueMinimum: finalObject[selectedExchange][value].min,
      assetValueError: false,
      selectedCurrency: value
    })
  }

  onAssetAdd = () => {
    const {
      selectedExchange,
      selectedCurrency,
      assetValue,
      finalObject
    } = this.state

    // validate
    var newFinalObject = Object.assign({}, finalObject)
    const curr = finalObject[selectedExchange][selectedCurrency]

    debugger;
    if (!assetValue.match(/^(0\.\d+)$|^([1-9]\d*(\.\d+)?)$/) || parseFloat(assetValue) < curr.min) { // Not OK
      this.setState({
        assetValueError: true,
      })
      return
    }

    newFinalObject[selectedExchange][selectedCurrency].value = parseFloat(assetValue)
    this.setState({
      assetValue: '',
      selectedCurrency: null,
      assetValueError: false,
      finalObject: newFinalObject,
      assetValueMinimum: 0,
      submitUnclocked: true
    })
  }

  onAssetRemove = (currency) => {
    const {
      selectedExchange,
      finalObject,
    } = this.state

    var newFinalObject = Object.assign({}, finalObject)

    if (finalObject[selectedExchange][currency].min > 0) {
      newFinalObject[selectedExchange][currency].value = newFinalObject[selectedExchange][currency].min
    } else {
      newFinalObject[selectedExchange][currency].value = 0
    }
    this.setState({
      finalObject: newFinalObject,
      submitUnclocked: true
    })
  }

  onSubmit = async () => {
    const {
      finalObject,
      selectedExchange
    } = this.state

    var assetsToSubmit = Object.keys(finalObject[selectedExchange]).map(o => finalObject[selectedExchange][o])
      .filter(o => o.value > 0).map(o => ({ currency: o.currValue, amount: parseFloat(o.value) }))
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
      selectedExchange,
      selectedCurrency,
      showMessage,
      error,
      assetValue,
      assetValueError,
      assetValueMinimum,
      finalObject,
      submitUnclocked
    } = this.state
    const {
      color,
      assetOptions,
      submitAssetsFetching,
      exchangesOverviewFetching,
    } = this.props

    const possibleExchanges = assetOptions
      ? assetOptions.map(o => ({ key: o.value, text: o.text, value: o.value }))
      : []
    const possibleCurrencies = selectedExchange
      ? assetOptions.find(o => o.value === selectedExchange).currencies
        .map(o => ({ key: o.value, text: o.text, value: o.value }))
      : []

    return (
      <Modal trigger={
        <Button
          disabled={exchangesOverviewFetching || possibleExchanges.length === 0}
          color={color}
          loading={exchangesOverviewFetching}
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
              {selectedExchange && Object.keys(finalObject[selectedExchange])
                .filter(o => finalObject[selectedExchange][o].value > 0)
                .map(o => {
                  const curr = finalObject[selectedExchange][o]
                  return (
                    <Label className='currencyLabel' color={color} key={curr.currValue}>
                      {`${curr.text} ${curr.value}`}
                      <Label.Detail>{o.taken}</Label.Detail>
                      { curr.min !== curr.value &&
                        <Icon name='close' onClick={() => this.onAssetRemove(curr.currValue)} />}
                    </Label>)
                })}
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
  assetOptions: PropTypes.array,
  assets: PropTypes.array,
  submitAssets: PropTypes.func,
  submitAssetsFetching: PropTypes.bool,
  exchangesOverviewFetching: PropTypes.bool,
}

export default FakeAssetManagerModal
