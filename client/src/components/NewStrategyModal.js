import React from 'react'
import PropTypes from 'prop-types'
import { Button, Header, Icon, Modal, Form, Message, Label } from 'semantic-ui-react'
import './styles/NewStrategyModalContent.scss'

class NewStrategyModal extends React.Component {
  constructor (props) {
    super(props)
    this.state = {
      selectedExchange: null,
      selectedCurrency: null,
      assetValueError: false,
      name: '',
      description: '',
      assetValue: '',
      submitUnlocked: false,
      returnedId: null,
      strategyAssets: []
    }
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value })
    if (name === 'assetValue') this.setState({ assetValueError: false })
  }

  onExchangeSelect = (e, { value }) => {
    if (value === this.state.selectedExchange) return
    this.setState({
      selectedExchange: value,
      assetValueError: false,
      selectedCurrency: null
    })
  }

  onCurrencySelect = (e, { value }) => {
    if (value === this.state.selectedCurrency) return
    const {
      strategyAssets,
      selectedExchange
    } = this.state
    const curr = selectedExchange
      ? strategyAssets.find(o => o.exchange === selectedExchange && o.currency === value)
      : null
    if (curr) this.setState({ assetValue: curr.amount })
    this.setState({
      assetValueError: false,
      selectedCurrency: value
    })
  }

  onAssetAdd = () => {
    const {
      selectedCurrency,
      selectedExchange,
      assetValue,
      strategyAssets
    } = this.state
    const allAssets = this.props.allAssets

    const asset = allAssets.find(o => o.id === selectedExchange).currencies.find(o => o.id === selectedCurrency)
    const amount = parseFloat(assetValue)
    if (!assetValue.match(/^(0\.\d+)$|^([1-9]\d*(\.\d+)?)$/) || amount > asset.free || amount <= 0) { // Not OK
      this.setState({
        assetValueError: true,
      })
      return
    }
    const curr = strategyAssets.find(o => o.exchange === selectedExchange && o.currency === selectedCurrency)
    const newAssets = curr ? strategyAssets.map(o => o.exchange === selectedExchange && o.currency === selectedCurrency
        ? { ...o, amount }
        : o)
      : [ ...strategyAssets, { exchange: selectedExchange, currency: selectedCurrency, amount } ]
    this.setState({
      strategyAssets: newAssets,
      assetValue: '',
      selectedCurrency: null,
      assetValueError: false,
      submitUnclocked: true
    })
  }

  onAssetRemove = (currency) => {
    const {
      strategyAssets,
      selectedExchange
    } = this.state

    this.setState({
      strategyAssets: strategyAssets.filter(o => o.exchange !== selectedExchange || o.currency !== currency),
      submitUnclocked: true
    })
  }

  onSubmit = async () => {
    const {
      name,
      description,
      selectedExchange,
      strategyAssets
    } = this.state

    const id = await this.props
      .registerStrategy(name, selectedExchange, description, strategyAssets.filter(o => o.exchange === selectedExchange))
    this.setState({ returnedId: id })
    if (id) this.cleanForm(false)
  }

  cleanForm = (deleteReturnedId) => {
    this.setState({
      name: '',
      description: '',
      selectedExchange: null,
      selectedCurrency: null,
      assetValueError: false,
      assetValue: '',
      strategyAssets: []
    })
    if (deleteReturnedId) this.setState({ returnedId: null })
  }

  render () {
    const {
      name,
      description,
      assetValue,
      assetValueError,
      selectedExchange,
      selectedCurrency,
      returnedId,
      submitUnclocked,
      strategyAssets
    } = this.state
    const {
      allAssets,
    } = this.props

    const possibleExchanges = allAssets
      ? allAssets.map(o => ({ key: o.id, text: o.name, value: o.id }))
      : []
    const exchange = selectedExchange
      ? allAssets.find(o => o.id === selectedExchange).currencies : null
    const possibleCurrencies = selectedExchange
      ? exchange.filter(o => o.free > 0)
        .map(o => ({ key: o.id, text: o.name, value: o.id }))
      : []
    const currency = selectedExchange && selectedCurrency
      ? exchange.find(o => o.id === selectedCurrency) : null
    return (
      <Modal
        trigger={<Button primary>New Strategy</Button>}
        closeIcon
        onClose={() => this.cleanForm(true)}
        onOpen={this.onModalOpen}>
        <Header icon='plug' content='New Strategy' />
        <Modal.Content className='newStrategyModalContent'>
          <Form loading={this.props.registrationPending}>
            <Form.Group>
              <Form.Input
                fluid
                width={11}
                value={name}
                label='Name'
                name='name'
                placeholder='Name'
                onChange={this.onInputChange} />
              <Form.Select
                fluid
                width={5}
                label='Exchange'
                placeholder='Exchange'
                name='exchange'
                value={selectedExchange}
                options={possibleExchanges}
                onChange={this.onExchangeSelect} />
            </Form.Group>
            <Form.TextArea
              label='Description'
              name='description'
              value={description}
              placeholder='Tell me more...'
              onChange={this.onInputChange} />
            <Form.Group widths='equal'>
              <Form.Select
                fluid
                label='Currency'
                name='assetCurrency'
                value={selectedCurrency}
                disabled={possibleCurrencies.length === 0 || selectedExchange === null}
                options={possibleCurrencies}
                placeholder='Currency'
                onChange={this.onCurrencySelect} />
              <Form.Input
                type='number'
                disabled={selectedCurrency === null}
                fluid
                error={assetValueError}
                label={`Value${currency ? ` (Maximum: ${currency.free}` : ''})`}
                value={assetValue}
                name='assetValue'
                onChange={this.onInputChange} />
              <Form.Button
                disabled={assetValue === ''}
                onClick={this.onAssetAdd}color='green'
                className='modalButton'>
                <Icon name='checkmark' size='large' />
              </Form.Button>
            </Form.Group>
          </Form>
          <div style={{ minHeight: '30px' }}>
            {selectedExchange && strategyAssets.filter(o => o.exchange === selectedExchange)
              .map(curr => {
                return (
                  <Label className='currencyLabel' key={curr.currency}>
                    {`${curr.currency} ${curr.amount}`}
                    <Icon name='close' onClick={() => this.onAssetRemove(curr.currency)} />
                  </Label>)
              })}
          </div>
          <Message
            onDismiss={() => this.setState({ returnedId: null })}
            success={returnedId !== null}
            error={returnedId === null}
            hidden={returnedId === null}
            header={returnedId ? `Strategy Registered. Id: ${returnedId}` : 'Error getting id'}
          />
        </Modal.Content>
        <Modal.Actions>
          <Button disabled={!submitUnclocked || !name || !description} color='green' onClick={() => this.onSubmit()}>
            <Icon name='checkmark' /> Register
          </Button>
        </Modal.Actions>
      </Modal>
    )
  }
}

NewStrategyModal.propTypes = {
  allAssets: PropTypes.array,
  registerStrategy: PropTypes.func,
  registrationPending: PropTypes.bool,
}

export default NewStrategyModal
