import React from 'react'
import { Button, Header, Modal, Message, Form, Icon, Label } from 'semantic-ui-react'
import styles from '../styles/modals.module.scss';

class FakeAssetManagerModal extends React.Component {
  constructor (props) {
    super(props)

    this.state = {
      selectedExchange: null,
      selectedCurrency: null,
      assetValue: '',
      modalOpened: false,
      exchangeChanges: false,
      isSubmitting: false,
      toSubmit: [],
    }
  }

  onModalOpen = () => {
    this.setState({ toSubmit: this.props.assets.filter(o => !o.strategyId).map(o => ({...o})) })
  }

  onInputChange = (e, { name, value }) => {
    this.setState({ [name]: value, assetValueError: false })
  }

  onExchangeSelect = (e, { value }) => {
    const { exchangeChanges } = this.state
    if(exchangeChanges) alert('Changes were made');
    if (value === this.state.selectedExchange) return;

    this.setState({
      selectedExchange: value,
      selectedCurrency: null,
      exchangeChanges: false,
    })
  }

  onCurrencySelect = (e, { value }) => {
    const {
      toSubmit,
      selectedExchange,
    } = this.state;
    const origin = toSubmit.find(o => o.exchange === selectedExchange && o.currency === value);
    this.setState({
      selectedCurrency: value,
      assetValue: origin ? origin.amount : '',
    });
  }

  onAssetAdd = () => {
    const {
      toSubmit,
      selectedExchange,
      selectedCurrency,
      assetValue,
    } = this.state;
    const origin = toSubmit.find(o => o.exchange === selectedExchange && o.currency === selectedCurrency);
    if(!origin){
      this.setState({toSubmit: [...toSubmit, {
        exchange: selectedExchange,
        currency: selectedCurrency,
        amount: assetValue,
        free: assetValue,
      }],
      exchangeChanges: true,
    })
    } else{
      this.setState({toSubmit: toSubmit.map(o => o === origin ? 
        ({
          ...o,
          amount: assetValue,
          free: o.free + (assetValue - o.amount)
        }) : o),
        exchangeChanges: true,
      })
    }
  }

  onAssetRemove = (currency) => {
    const {
      selectedExchange,
      toSubmit,
    } = this.state;
    this.setState({toSubmit: toSubmit.map(o => o.exchange === selectedExchange && o.currency === currency ? 
      ({
        ...o,
        amount: Math.max(0, o.amount - o.free),
        free: 0,
      }) : o),
      exchangeChanges: true,
    }, () => this.onCurrencySelect(null, {value: currency}))
  }

  onSubmit = async () => {
    const { toSubmit } = this.state
    this.setState({isSubmitting: true});
    const res = await this.props.submitAssets(toSubmit.filter(o => o.amount > 0))
    this.setState({isSubmitting: false});
    if(res.value) this.setState({modalOpened: false});
  }

  cleanForm = () => {
    this.setState({
      selectedExchange: null,
      selectedCurrency: null,
      assetValue: '',
      modalOpened: false,
    })
  }

  render () {
    const {
      toSubmit,
      selectedExchange,
      selectedCurrency,
      assetValue,
      exchangeChanges,
      modalOpened,
      isSubmitting,
    } = this.state
    const {
      color,
      marketData,
      marketDataFetching,
    } = this.props

    const possibleExchanges = marketData
      ? marketData.map(o => ({ key: o.id, text: o.name, value: o.id }))
      : []
    const possibleCurrencies = selectedExchange
      ? marketData.find(o => o.id === selectedExchange).currencies
        .map(o => ({ key: o.id, text: o.id, value: o.id }))
      : [];

    return (
      <Modal
        trigger={
          <Button
            disabled={marketDataFetching || possibleExchanges.length === 0}
            color={color}
            loading={marketDataFetching}
            onClick={() => this.setState({modalOpened: true})}
          >
            Manage assets
          </Button>
        } 
        closeIcon 
        onClose={this.cleanForm} 
        size='small'
        open={modalOpened}
        onOpen={this.onModalOpen}
        >
        <Header icon='money' content='Asset Management' />
        <Modal.Content>
          <div>
            <Form widths='equal' loading={isSubmitting}>
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
                  label={`Value`}
                  value={assetValue || ''}
                  name='assetValue'
                  width={5}
                  onChange={this.onInputChange} />
                <Form.Button
                  disabled={assetValue === ''}
                  onClick={this.onAssetAdd}
                  color='green'
                  width={2}
                  className={styles.form_button}>
                  <Icon name='checkmark' size='large' />
                </Form.Button>
              </Form.Group>
            </Form>
            { selectedExchange && (<h5 classNam={styles.no_margin}>Free assets</h5>)}
            <div className={styles.assets_wrapper}>
              {toSubmit.filter(o => o.exchange === selectedExchange)
                .map(o => (
                  <Label color={color} key={o.id} onClick={(e) => this.onCurrencySelect(null, {value: o.currency})}>
                    {`${o.currency} ${o.amount}`}
                    { o.free > 0 &&
                      <Icon name='close' onClick={(e) => {
                        e.stopPropagation();
                        this.onAssetRemove(o.currency)}
                       } 
                      />}
                  </Label>
                ))}
            </div>
          </div>
        </Modal.Content>
        <Modal.Actions>
          <Button
            disabled={!exchangeChanges}
            onClick={this.onSubmit}
            color={color}
          >
            Submit Assets
          </Button>
        </Modal.Actions>
      </Modal>
    )
  }
}

export default FakeAssetManagerModal
