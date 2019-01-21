import CounterExample from 'components/counter-example'
import FetchData from 'components/fetch-data'
import HomePage from 'components/home-page'
import DetailPage from 'components/detail'
import AboutUs from 'components/about-us'

export const routes = [
  { name: 'home', path: '/', component: HomePage, display: 'Home', icon: 'home' },
  { name: 'detail', path: '/detail/:id', component: DetailPage, display: 'Detail' },
  { name: 'counter', path: '/counter', component: CounterExample, display: 'Counter', icon: 'graduation-cap' },
  { name: 'fetch-data', path: '/fetch-data', component: FetchData, display: 'Fetch data', icon: 'list' },
  { name: 'about-us', path: '/about-us', component: AboutUs, display: 'AboutUs', icon: 'about-us' }
]
